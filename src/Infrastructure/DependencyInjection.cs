using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using System.Security;

namespace Infrastructure;

//We will call this class in theProgram.cs file to register our
//infrastructure services with the dependency injection container.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        //Add DbContext to the service collection, using PostgreSQL and the connection string from the configuration.
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        //Add Identity services to the service collection, using our ApplicationUser class
        services.AddIdentityCore<ApplicationUser>(opt =>
        {

            //password requirements non-alphanumeric characters, length of at least 8 characters
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequiredLength = 8;
            //require unique email for each user
            opt.User.RequireUniqueEmail = true;
        })
            //IdentityRole is a class provided by ASP.NET Core Identity that represents a role in the system.
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


        // return all registered services to allow for method chaining in the Program.cs file.
        return services;
    }
}
