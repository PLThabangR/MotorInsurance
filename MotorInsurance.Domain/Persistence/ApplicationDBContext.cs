
using Microsoft.EntityFrameworkCore;
using MotorInsurance.Domain.Entities;


namespace MotorInsurance.Domain.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//DBSet represents a table in the database
//EF Core will create a table named Users

    //DBSey<T> represents a table
    public DbSet<User> Users { get; set; }


    //Configure the database schema and relationships
    //Fluent API to define table names ,indexes,contstraints 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Map the User entity to the Users table
        modelBuilder.Entity<User>(entity =>
        {
            //Table name
            entity.ToTable("Users");
            //Propertiy configuration
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd();//Auto increment

            //Database validation
            entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

            //Setting constraints index help for faster search
            entity.HasIndex(e => e.Email)
    .IsUnique();

        

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            //Defualt value 
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

   
    }

    // ==========================================
    // ADD THIS: Configure migrations assembly
    // ==========================================
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;Database=MotorInsuranceDb;User Id=admin;Password=P@ssw0rd123!;",
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("MotorInsurance.API")
            );
        }

    }
    
}