using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    //Creating a DbContext for the AppDbContext,
    //inheriting from IdentityDbContext to include ASP.NET Core Identity tables
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //DbSets for each entity in the domain model
    public DbSet<PolicyHolder> PolicyHolders => Set<PolicyHolder>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Claim> Claims => Set<Claim>();
    public DbSet<TelematicsRecord> TelematicsRecords => Set<TelematicsRecord>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder is an EF Core object used to configure your database model.
        base.OnModelCreating(modelBuilder);
        

        //Setting constraints for PolicyHolder tables
        modelBuilder.Entity<PolicyHolder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();//Ensuring that each policy holder is linked to a user
            entity.HasIndex(e => e.UserId).IsUnique();//Ensuring that each policy holder is linked to a unique user
            entity.Property(e => e.FirstName).IsRequired();//Ensuring that each policy holder has a first name
            entity.Property(e => e.LastName).IsRequired();//Ensuring that each policy holder has a last name
            entity.Property(e => e.Email).IsRequired();//Ensuring that each policy holder has an email address

        });

        //Setting constraints for Policy tables
        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PolicyNumber).IsRequired().HasMaxLength(50);//Ensuring that each policy has a policy number
            entity.HasIndex(e => e.PolicyNumber).IsUnique();//Ensuring that each policy has a unique policy number
            entity.Property(e => e.Premium).HasPrecision(18, 2);//Ensuring that each policy has a premium with a precision of 18 and scale of 2
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);//Ensuring that each policy has a currency with a maximum length of 3
            entity.Property(e => e.Status).HasConversion<string>();//Ensuring that each policy has a status that is stored as a string in the database
            entity.HasOne(e => e.PolicyHolder)//One policy holder can have many policies, but each policy is linked to one policy holder. This is a one-to-many relationship.
                  .WithMany(p => p.Policies)
                  .HasForeignKey(e => e.PolicyHolderId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.PolicyHolderId);// Ensure quiker searching of policies by policy holder
        });

        //Setting constraints for Vehicle tables
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RegistrationNumber).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.RegistrationNumber);
            entity.Property(e => e.Make).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Model).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Vin).IsRequired().HasMaxLength(17);
            entity.HasOne(e => e.Policy)//One policy can have many vehicles, but each vehicle is linked to one policy. This is a one-to-many relationship.
                  .WithMany(p => p.Vehicles)
                  .HasForeignKey(e => e.PolicyId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.PolicyId);// Ensure quiker searching of vehicles by policy

        });

        //Setting constraints for Driver tables
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Policy) //One policy can have many drivers, but each driver is linked to one policy. This is a one-to-many relationship.
                  .WithMany(p => p.Drivers)
                  .HasForeignKey(e => e.PolicyId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        //Setting constraints for Claim tables
        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClaimNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ClaimNumber).IsUnique();//Ensuring that each claim has a unique claim number
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.EstimatedAmount).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasConversion<string>();//Ensuring that each claim has a status that is stored as a string in the database
            entity.HasOne(e => e.Policy)
                  .WithMany(p => p.Claims)
                  .HasForeignKey(e => e.PolicyId);
        });

        //Setting constraints for TelematicsRecord tables
        modelBuilder.Entity<TelematicsRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Vehicle)
                  .WithMany(v => v.TelematicsRecords)//One vehicle can have many telematics records, but each telematics record is linked to one vehicle. This is a one-to-many relationship.
                  .HasForeignKey(e => e.VehicleId);
        });

        //setting constraints for refrence data tables
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
           entity.HasIndex(e => e.TokenHash, "IX_RefreshTokens_TokenHash").IsUnique();//Unique index on token hash

        });
    }

}
