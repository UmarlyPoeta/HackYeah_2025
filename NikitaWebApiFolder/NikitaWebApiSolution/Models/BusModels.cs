using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NikitaWebApiSolution.Models
{
    public class BusDbContext : DbContext
    {
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusReport> BusReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bustracking.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для Bus
            modelBuilder.Entity<Bus>(entity =>
            {
                entity.HasIndex(b => b.LicensePlate).IsUnique();
                entity.Property(b => b.Status).HasConversion<int>();

                // Seed data для автобусов
                entity.HasData(
                    new Bus
                    {
                        Id = 1,
                        LicensePlate = "А123АА777",
                        Model = "PAZ-3205",
                        Capacity = 45,
                        BusNumber = "101",
                        CurrentLatitude = 55.7558,
                        CurrentLongitude = 37.6173,
                        Speed = null,
                        Bearing = null,
                        Status = BusStatus.Active,
                        LastUpdate = DateTime.UtcNow
                    },
                    new Bus
                    {
                        Id = 2,
                        LicensePlate = "В456ВВ777",
                        Model = "LiAZ-5292",
                        Capacity = 85,
                        BusNumber = "205",
                        CurrentLatitude = 55.7517,
                        CurrentLongitude = 37.6178,
                        Speed = null,
                        Bearing = null,
                        Status = BusStatus.Active,
                        LastUpdate = DateTime.UtcNow
                    },
                    new Bus
                    {
                        Id = 3,
                        LicensePlate = "С789СС777",
                        Model = "MAZ-203",
                        Capacity = 90,
                        BusNumber = "156",
                        CurrentLatitude = null,
                        CurrentLongitude = null,
                        Speed = null,
                        Bearing = null,
                        Status = BusStatus.Maintenance,
                        LastUpdate = DateTime.UtcNow.AddHours(-2)
                    }
                );
            });

            // Конфигурация для BusReport
            modelBuilder.Entity<BusReport>(entity =>
            {
                entity.Property(r => r.CrowdingLevel).HasConversion<int>();
                entity.Property(r => r.VehicleFailure).HasConversion<int>();
                entity.Property(r => r.AirConditioning).HasConversion<int>();
                entity.Property(r => r.SmellLevel).HasConversion<int>();
            });
        }
    }

    // Остальные модели остаются без изменений
    public class Bus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(1, 200)]
        public int Capacity { get; set; }

        [Required]
        [StringLength(10)]
        public string BusNumber { get; set; } = string.Empty;

        public double? CurrentLatitude { get; set; }
        public double? CurrentLongitude { get; set; }
        public double? Speed { get; set; }
        public double? Bearing { get; set; }

        [Required]
        public BusStatus Status { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }
    }

    public enum BusStatus
    {
        Active = 0,
        Inactive = 1,
        Maintenance = 2,
        OutOfService = 3
    }

    public class BusReport
    {
        public int Id { get; set; }

        [Required]
        public int BusNumber { get; set; }

        [Required]
        public CrowdingLevel CrowdingLevel { get; set; }

        public int? DelayMinutes { get; set; }

        public VehicleFailure? VehicleFailure { get; set; }

        public AirConditioning? AirConditioning { get; set; }

        public SmellLevel? SmellLevel { get; set; }

        public string? AdditionalComments { get; set; }

        [Required]
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;

        public string? UserIP { get; set; }
    }

    public enum CrowdingLevel
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Urgent = 3
    }

    public enum VehicleFailure
    {
        Temporary = 0,
        Complete = 1
    }

    public enum AirConditioning
    {
        Freezing = 0,
        Cool = 1,
        Good = 2,
        Warm = 3,
        Oven = 4
    }

    public enum SmellLevel
    {
        Violets = 0,
        LightSmell = 1,
        Smelly = 2,
        VerySmelly = 3
    }
}