using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MotorInsurance.Domain.Persistence;

namespace MotorInsurance.API.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
           "Host=localhost;Port=5432;Database=MotorInsuranceDb;Username=postgres;Password=postgres;",

npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure();
                // ==========================================
                // ADD THIS LINE
                // ==========================================
                npgsqlOptions.MigrationsAssembly("MotorInsurance.API");
            }


        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}