using CarRental.Domain.Data;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, CarRentalFixture seed) : DbContext(options)
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<CarModel> CarModels { get; set; }
    public DbSet<ModelGeneration> ModelGenerations { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CarModel>().HasKey(m => m.Id);

        modelBuilder.Entity<ModelGeneration>(e =>
        {
            e.HasKey(mg => mg.Id);
            e.Property(mg => mg.RentalPricePerHour).HasColumnType("decimal(18,2)");
            e.HasOne(mg => mg.Model)
             .WithMany()
             .HasForeignKey(mg => mg.ModelId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Car>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasOne(c => c.ModelGeneration)
             .WithMany()
             .HasForeignKey(c => c.ModelGenerationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Client>().HasKey(c => c.Id);

        modelBuilder.Entity<Rental>(e =>
        {
            e.HasKey(r => r.Id);
            e.HasOne(r => r.Car)
             .WithMany()
             .HasForeignKey(r => r.CarId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.Client)
             .WithMany()
             .HasForeignKey(r => r.ClientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        modelBuilder.Entity<CarModel>().HasData(seed.CarModels);
        modelBuilder.Entity<ModelGeneration>().HasData(seed.ModelGenerations);
        modelBuilder.Entity<Car>().HasData(seed.Cars);
        modelBuilder.Entity<Client>().HasData(seed.Clients);
        modelBuilder.Entity<Rental>().HasData(seed.Rentals);
    }
}
