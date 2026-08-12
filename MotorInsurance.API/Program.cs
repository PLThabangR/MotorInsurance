using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MotorInsurance.Domain.Persistence;
using Serilog;
using System.Reflection;

// ==========================================
// 1. CONFIGURE SERILOG (Structured Logging)
// ==========================================
// We configure Serilog BEFORE building the app
// This ensures we can log startup errors
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()    // Only log Information and above
    .WriteTo.Console()             // Output to console (Docker logs)
    .WriteTo.File(                 // Output to file
        "logs/log-.txt",           // File path with date
        rollingInterval: RollingInterval.Day, // New file each day
        retainedFileCountLimit: 7  // Keep 7 days of logs
    )
    .CreateLogger();

try
{
    // ==========================================
    // 2. BUILD THE APPLICATION
    // ==========================================
    var builder = WebApplication.CreateBuilder(args);

    // ==========================================
    // ADD DATABASE CONTEXT
    // ==========================================
    // Register ApplicationDbContext with dependency injection
    // Use PostgreSQL as the database provider
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {

        //get connection string
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        //use the connection string
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            //enable retry on connection failures
            npgsqlOptions.EnableRetryOnFailure(); //Resilience

            //set the command timeout
            npgsqlOptions.CommandTimeout(30);
            // Specify that migrations should be created in this  project
            npgsqlOptions.MigrationsAssembly("MotorInsurance.API");
        });



    });


    // 2.1 - Basic ASP.NET Core services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // 2.2 - MEDIATR: Register all handlers
    // Scans the current assembly for IRequestHandler implementations
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

    // 2.3 - CARTER: Register endpoint modules
    // Scans for classes implementing ICarterModule
    builder.Services.AddCarter();

    // 2.4 - HEALTH CHECKS: For monitoring
    // Used by Docker and Kubernetes to check if app is healthy
    builder.Services.AddHealthChecks();

    // 2.5 - SERILOG: Use as the logging provider
    builder.Host.UseSerilog();

    // ==========================================
    // 3. BUILD AND CONFIGURE THE PIPELINE
    // ==========================================
    var app = builder.Build();

    // 3.1 - DATABASE MIGRATIONS
    using (var scope = app.Services.CreateScope())
    {   //  Database migration
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


        if (app.Environment.IsDevelopment())
        {
            // SEED DATA - Why?
            // We need some test data to work with
            // In production, this would come from a database
            dbContext.Database.Migrate();
            Log.Information("Database migrations applied successfully");

        } //end of if
    } //end of using scope


    // - Development tools
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();      // Generate OpenAPI spec
        app.UseSwaggerUI();    // Swagger UI for testing
    }


    // 3.2 - Security middleware
    app.UseHttpsRedirection();  // Force HTTPS (for production)
    app.UseAuthorization();     // Authorization (JWT coming later)

    // 3.3 - Health check endpoint
    app.MapHealthChecks("/health");

    // 3.4 - Carter endpoints
    app.MapCarter();            // Maps all registered modules

    // 3.5 - Run the application
    app.Run();
}
catch (Exception ex)
{
    // CATCH AND LOG startup failures
    Log.Fatal(ex, "Application startup failed");
    throw;
}
finally
{
    // FLUSH logs before exit
    Log.CloseAndFlush();
}