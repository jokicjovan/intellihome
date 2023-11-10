using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Data.Models.Home;
using Data.Models.PKA;
using Data.Models.SPU;
using Data.Models.Users;
using Data.Models.VEU;
using System.Diagnostics.Contracts;

namespace Data.Context
{
    public class PostgreSqlDbContext : DbContext
    {
        public readonly IConfiguration _configuration;

        public DbSet<City> Cities { get; set; }
        public DbSet<SmartHome> SmartHomes { get; set; }
        public DbSet<SmartHomeApproveRequest> SmartHomeApproveRequests { get; set; }
        public DbSet<AirConditioner> AirConditioners { get; set; }
        public DbSet<AirConditionerWork> AirConditionerWorks { get; set; }
        public DbSet<AmbientSensor> AmbientSensors { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<WashingMachine> WashingMachines { get; set; }
        public DbSet<WashingMachineMode> WashingMachineModes { get; set; }
        public DbSet<Lamp> Lamps { get; set; }
        public DbSet<Sprinkler> Sprinklers { get; set; }
        public DbSet<SprinklerWork> SprinklerWorks { get; set; }
        public DbSet<VehicleGate> VehicleGates { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Battery> Batteries { get; set; }
        public DbSet<BatterySystem> BatterySystems { get; set; }
        public DbSet<SolarPanel> SolarPanels { get; set; }
        public DbSet<SolarPanelSystem> SolarPanelSystems { get; set; }
        public DbSet<VehicleCharger> VehicleChargers { get; set; }
        public DbSet<VehicleChargingPoint> VehicleChargingPoints { get; set; }

        public PostgreSqlDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("IntelliHomeSQLDatabase"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>()
                .HasIndex(e => new { e.Email })
                .IsUnique(true);

            modelBuilder.Entity<Admin>()
                .HasIndex(e => new { e.Username })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(e => new { e.Email })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(e => new { e.Username })
                .IsUnique(true);

            modelBuilder.Entity<BaseUser>()
            .ToTable("Users")
            .HasDiscriminator<string>("Discriminator")
            .HasValue<User>("User")
            .HasValue<Admin>("Admin");

            modelBuilder.Entity<AirConditioner>().ToTable("AirConditioners");
            modelBuilder.Entity<AmbientSensor>().ToTable("AmbientSensors");
            modelBuilder.Entity<WashingMachine>().ToTable("WashingMachines");
            modelBuilder.Entity<Lamp>().ToTable("Lamps");
            modelBuilder.Entity<Sprinkler>().ToTable("Sprinklers");
            modelBuilder.Entity<VehicleGate>().ToTable("VehicleGates");
            modelBuilder.Entity<BatterySystem>().ToTable("BatterySystems");
            modelBuilder.Entity<SolarPanelSystem>().ToTable("SolarPanelSystems");
            modelBuilder.Entity<VehicleCharger>().ToTable("VehicleChargers");
            modelBuilder.Entity<Confirmation>().ToTable("Confirmations");

            modelBuilder.Entity<WashingMachineMode>().ToTable("WashingMachineModes");
            modelBuilder.Entity<WashingMachine>()
                .HasMany(wm => wm.Modes)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "WashingMachineWashingMachineMode",
                    j => j.HasOne<WashingMachineMode>().WithMany(),
                    j => j.HasOne<WashingMachine>().WithMany()
                );

        }
    }
}
