using Azure;
using Ecomove.Api.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecomove.Api.Data;

public class EcoMoveDbContext : IdentityDbContext<AppUser>
{
    private string connectionString =
        "server=localhost;database=EcomoveDB;user=root;password=pass123";
    public DbSet<Brand> Brands { get; set; }
    public DbSet<CarpoolAddress> CarpoolAddresses { get; set; }
    public DbSet<CarpoolAnnouncement> CarpoolAnnouncements { get; set; }
    public DbSet<CarpoolBooking> CarpoolBookings { get; set; }
    public DbSet<Motorization> Motorizations { get; set; }
    public DbSet<RentalVehicle> RentalVehicles { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Status> Status { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    public EcoMoveDbContext(DbContextOptions<EcoMoveDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<CarpoolAnnouncement>()
            .HasOne(m => m.PickupAddress)
            .WithMany(p => p.CarpoolAnnouncementsPickUp)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Entity<CarpoolAnnouncement>()
            .HasOne(m => m.DropOffAddress)
            .WithMany(p => p.CarpoolAnnouncementsDropOff)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Entity<CarpoolBooking>()
            .HasKey(rv => new { rv.CarpoolAnnouncementId, rv.AppUserId });

        builder
            .Entity<AppUser>()
            .HasOne(e => e.Role) // Un utilisateur a un seul rôle
            .WithMany() // Un rôle peut être lié à plusieurs utilisateurs
            .HasForeignKey(e => e.RoleId); // Clé étrangère dans AppUser

        //// Création des deux rôles en BDD
        //IdentityRole roleAdmin = new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" };
        //IdentityRole roleUser = new IdentityRole { Name = "USER", NormalizedName = "USER" };
        //builder.Entity<IdentityRole>().HasData(new List<IdentityRole> { roleAdmin, roleUser });

        // Fixtures
        Brand peugeot = new Brand { BrandId = 1, BrandLabel = "Peugeot" };
        Brand renault = new Brand { BrandId = 2, BrandLabel = "Renault" };
        Brand citroen = new Brand { BrandId = 3, BrandLabel = "Citroen" };
        Brand mercedes = new Brand { BrandId = 4, BrandLabel = "Mercedes" };
        builder.Entity<Brand>().HasData(new List<Brand> { peugeot, renault, citroen, mercedes });

        Model p206 = new Model
        {
            ModelId = 1,
            ModelLabel = "206",
            BrandId = peugeot.BrandId,
        };
        Model p308 = new Model
        {
            ModelId = 2,
            ModelLabel = "308",
            BrandId = peugeot.BrandId,
        };
        Model p2008 = new Model
        {
            ModelId = 3,
            ModelLabel = "2008",
            BrandId = peugeot.BrandId,
        };
        Model p508 = new Model
        {
            ModelId = 4,
            ModelLabel = "508",
            BrandId = peugeot.BrandId,
        };
        Model r5 = new Model
        {
            ModelId = 5,
            ModelLabel = "Renault 5",
            BrandId = renault.BrandId,
        };
        Model clio = new Model
        {
            ModelId = 6,
            ModelLabel = "Clio",
            BrandId = renault.BrandId,
        };
        Model megane = new Model
        {
            ModelId = 7,
            ModelLabel = "Megane",
            BrandId = renault.BrandId,
        };
        Model c4 = new Model
        {
            ModelId = 8,
            ModelLabel = "C4",
            BrandId = citroen.BrandId,
        };
        builder
            .Entity<Model>()
            .HasData(new List<Model> { p206, p308, p2008, p508, r5, clio, megane, c4 });

        Category suv = new Category { CategoryId = 1, CategoryLabel = "SUV" };
        Category berline = new Category { CategoryId = 2, CategoryLabel = "Berline" };
        Category citadine = new Category { CategoryId = 3, CategoryLabel = "Citadine" };
        Category monospace = new Category { CategoryId = 4, CategoryLabel = "Monospace" };
        builder
            .Entity<Category>()
            .HasData(new List<Category> { suv, berline, citadine, monospace });

        Motorization essence = new Motorization
        {
            MotorizationId = 1,
            MotorizationLabel = "Essence",
        };
        Motorization hybride = new Motorization
        {
            MotorizationId = 2,
            MotorizationLabel = "Hybride",
        };
        Motorization electric = new Motorization
        {
            MotorizationId = 3,
            MotorizationLabel = "Éléctrique",
        };
        Motorization gazoil = new Motorization
        {
            MotorizationId = 4,
            MotorizationLabel = "Gazoile",
        };
        builder
            .Entity<Motorization>()
            .HasData(new List<Motorization> { essence, hybride, electric, gazoil });

        Status InOperation = new Status { StatusId = 1, StatusLabel = StatusEnum.InOperation };
        Status OutOfService = new Status { StatusId = 2, StatusLabel = StatusEnum.OutOfService };
        Status UnderRepair = new Status { StatusId = 3, StatusLabel = StatusEnum.UnderRepair };
        builder
            .Entity<Status>()
            .HasData(new List<Status> { InOperation, OutOfService, UnderRepair });

        Vehicle vehicle206 = new Vehicle
        {
            VehicleId = Guid.Parse("3454E9E3-BE2F-14D7-14D7-08DC8ABF2A7F"), // Conversion de string à Guid
            ModelId = 1,
            MotorizationId = 4,
            StatusId = 1,
            Registration = "SD-267-AZ",
            CategoryId = 3,
            CO2emission = 90,
            CarSeatNumber = 5,
            Consumption = 6.5,
            Photo =
                "https://images.caradisiac.com/images/2/6/7/9/192679/S1-peugeot-206-s16-1999-2005-la-gti-qui-ne-dit-pas-son-nom-des-2-500-eur-694126.jpg",
        };
        Vehicle vehicleClio = new Vehicle
        {
            VehicleId = Guid.Parse("6554E9E3-BE2F-45D0-14D7-08DC8ABF2A7F"), // Conversion de string à Guid
            ModelId = 6,
            MotorizationId = 1,
            StatusId = 1,
            Registration = "KN-324-LM",
            CategoryId = 3,
            CO2emission = 95,
            CarSeatNumber = 5,
            Consumption = 8.5,
            Photo =
                "https://images.caradisiac.com/logos/8/6/0/3/268603/S8-renault-clio-comment-le-prix-de-base-s-est-envole-en-deux-ans-192370.jpg",
        };
        builder.Entity<Vehicle>().HasData(new List<Vehicle> { vehicle206, vehicleClio });

        base.OnModelCreating(builder);
    }
}
