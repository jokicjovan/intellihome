using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Data.Models.Home;
using Data.Models.PKA;
using Data.Models.SPU;
using Data.Models.Users;
using Data.Models.VEU;

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
                .HasIndex(c => new { c.Email })
                .IsUnique(true);

            modelBuilder.Entity<Admin>()
                .HasIndex(c => new { c.Username })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(c => new { c.Email })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(c => new { c.Username })
            .IsUnique(true);

            // Dummy data
            City city = new City(Guid.NewGuid(), "Novi Sad", "Serbia", "21000");
            modelBuilder.Entity<City>().HasData(city);

            User user = new User(Guid.NewGuid(), "User1", "User1", "user1@example.com", "user1", BCrypt.Net.BCrypt.HashPassword("user1"), null);
            modelBuilder.Entity<User>().HasData(user);
            
            //SmartHome smartHome = new SmartHome(Guid.NewGuid(), "Lepa kuca", "Mise Dimitrijevica 124", city, "50", SmartHomeType.HOUSE, 2, null, 45.244320, 19.831070, true, user);
            //modelBuilder.Entity<SmartHome>().HasData(smartHome);
        }
    }
}
