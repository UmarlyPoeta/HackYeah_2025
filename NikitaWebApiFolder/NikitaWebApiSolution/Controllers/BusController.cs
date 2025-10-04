using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace BusTrackingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusesController : ControllerBase
    {
        private readonly BusDbContext _context;

        public BusesController(BusDbContext context)
        {
            _context = context;
        }

        // GET: api/buses
        [HttpGet]
        public async Task<IActionResult> GetAllBuses()
        {
            var buses = await _context.Buses.ToListAsync();

            var result = buses.Select(b => new BusResponse
            {
                Id = b.Id,
                LicensePlate = b.LicensePlate,
                Model = b.Model,
                Capacity = b.Capacity,
                BusNumber = b.BusNumber,
                CurrentLatitude = b.CurrentLatitude,
                CurrentLongitude = b.CurrentLongitude,
                Speed = b.Speed,
                Bearing = b.Bearing,
                Status = b.Status.ToString(),
                LastUpdate = b.LastUpdate
            }).ToList();

            return Ok(new { success = true, data = result });
        }

        // GET: api/buses/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveBuses()
        {
            var activeBuses = await _context.Buses
                .Where(b => b.Status == BusStatus.Active &&
                           b.CurrentLatitude.HasValue &&
                           b.CurrentLongitude.HasValue)
                .ToListAsync();

            var result = activeBuses.Select(b => new BusResponse
            {
                Id = b.Id,
                LicensePlate = b.LicensePlate,
                Model = b.Model,
                Capacity = b.Capacity,
                BusNumber = b.BusNumber,
                CurrentLatitude = b.CurrentLatitude,
                CurrentLongitude = b.CurrentLongitude,
                Speed = b.Speed,
                Bearing = b.Bearing,
                Status = b.Status.ToString(),
                LastUpdate = b.LastUpdate
            }).ToList();

            return Ok(new { success = true, data = result });
        }

        // GET: api/buses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound(new { success = false, message = "Bus not found" });
            }

            var result = new BusResponse
            {
                Id = bus.Id,
                LicensePlate = bus.LicensePlate,
                Model = bus.Model,
                Capacity = bus.Capacity,
                BusNumber = bus.BusNumber,
                CurrentLatitude = bus.CurrentLatitude,
                CurrentLongitude = bus.CurrentLongitude,
                Speed = bus.Speed,
                Bearing = bus.Bearing,
                Status = bus.Status.ToString(),
                LastUpdate = bus.LastUpdate
            };

            return Ok(new { success = true, data = result });
        }

        // POST: api/buses
        [HttpPost]
        public async Task<IActionResult> CreateBus([FromBody] CreateBusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            // Check for unique license plate
            if (await _context.Buses.AnyAsync(b => b.LicensePlate == request.LicensePlate))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Bus with this license plate already exists"
                });
            }

            var bus = new Bus
            {
                LicensePlate = request.LicensePlate,
                Model = request.Model,
                Capacity = request.Capacity,
                BusNumber = request.BusNumber,
                Status = BusStatus.Inactive,
                LastUpdate = DateTime.UtcNow
            };

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();

            var response = new BusResponse
            {
                Id = bus.Id,
                LicensePlate = bus.LicensePlate,
                Model = bus.Model,
                Capacity = bus.Capacity,
                BusNumber = bus.BusNumber,
                CurrentLatitude = bus.CurrentLatitude,
                CurrentLongitude = bus.CurrentLongitude,
                Speed = bus.Speed,
                Bearing = bus.Bearing,
                Status = bus.Status.ToString(),
                LastUpdate = bus.LastUpdate
            };

            return CreatedAtAction(nameof(GetBus), new { id = bus.Id },
                new { success = true, data = response });
        }

        // PUT: api/buses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBus(int id, [FromBody] UpdateBusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound(new { success = false, message = "Bus not found" });
            }

            // Check for unique license plate (excluding current bus)
            if (await _context.Buses.AnyAsync(b => b.LicensePlate == request.LicensePlate && b.Id != id))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Bus with this license plate already exists"
                });
            }

            bus.LicensePlate = request.LicensePlate;
            bus.Model = request.Model;
            bus.Capacity = request.Capacity;
            bus.BusNumber = request.BusNumber;
            bus.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new BusResponse
            {
                Id = bus.Id,
                LicensePlate = bus.LicensePlate,
                Model = bus.Model,
                Capacity = bus.Capacity,
                BusNumber = bus.BusNumber,
                CurrentLatitude = bus.CurrentLatitude,
                CurrentLongitude = bus.CurrentLongitude,
                Speed = bus.Speed,
                Bearing = bus.Bearing,
                Status = bus.Status.ToString(),
                LastUpdate = bus.LastUpdate
            };

            return Ok(new { success = true, data = response });
        }

        // PUT: api/buses/5/location
        [HttpPut("{id}/location")]
        public async Task<IActionResult> UpdateBusLocation(int id, [FromBody] UpdateLocationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound(new { success = false, message = "Bus not found" });
            }

            bus.CurrentLatitude = request.Latitude;
            bus.CurrentLongitude = request.Longitude;
            bus.Speed = request.Speed;
            bus.Bearing = request.Bearing;
            bus.Status = BusStatus.Active;
            bus.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Location updated successfully",
                data = new
                {
                    bus.Id,
                    bus.CurrentLatitude,
                    bus.CurrentLongitude,
                    bus.Speed,
                    bus.Bearing,
                    bus.LastUpdate
                }
            });
        }

        // PUT: api/buses/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBusStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound(new { success = false, message = "Bus not found" });
            }

            if (!Enum.IsDefined(typeof(BusStatus), request.Status))
            {
                return BadRequest(new { success = false, message = "Invalid status" });
            }

            bus.Status = request.Status;
            bus.LastUpdate = DateTime.UtcNow;

            // If status is not Active, reset coordinates
            if (request.Status != BusStatus.Active)
            {
                bus.CurrentLatitude = null;
                bus.CurrentLongitude = null;
                bus.Speed = null;
                bus.Bearing = null;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Status updated successfully",
                data = new
                {
                    bus.Id,
                    Status = bus.Status.ToString(),
                    bus.LastUpdate
                }
            });
        }

        // DELETE: api/buses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound(new { success = false, message = "Bus not found" });
            }

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Bus deleted successfully" });
        }

        // GET: api/buses/search?licensePlate=ABC123
        [HttpGet("search")]
        public async Task<IActionResult> SearchBuses([FromQuery] string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                return BadRequest(new { success = false, message = "License plate is required" });
            }

            var buses = await _context.Buses
                .Where(b => b.LicensePlate.Contains(licensePlate))
                .ToListAsync();

            var result = buses.Select(b => new BusResponse
            {
                Id = b.Id,
                LicensePlate = b.LicensePlate,
                Model = b.Model,
                Capacity = b.Capacity,
                BusNumber = b.BusNumber,
                CurrentLatitude = b.CurrentLatitude,
                CurrentLongitude = b.CurrentLongitude,
                Speed = b.Speed,
                Bearing = b.Bearing,
                Status = b.Status.ToString(),
                LastUpdate = b.LastUpdate
            }).ToList();

            return Ok(new { success = true, data = result });
        }
    }

    // Data models
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
    public class BusDbContext : DbContext
    {
        public DbSet<Bus> Buses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bustracking.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bus>()
                .HasIndex(b => b.LicensePlate)
                .IsUnique();

            modelBuilder.Entity<Bus>()
                .Property(b => b.Status)
                .HasConversion<int>();

            // Seed data
            modelBuilder.Entity<Bus>().HasData(
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
        }
    }

    // Request and Response DTOs
    public class CreateBusRequest
    {
        [Required(ErrorMessage = "License plate is required")]
        [StringLength(20, ErrorMessage = "License plate cannot exceed 20 characters")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 200, ErrorMessage = "Capacity must be between 1 and 200")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Bus number is required")]
        [StringLength(10, ErrorMessage = "Bus number cannot exceed 10 characters")]
        public string BusNumber { get; set; } = string.Empty;
    }

    public class UpdateBusRequest
    {
        [Required(ErrorMessage = "License plate is required")]
        [StringLength(20, ErrorMessage = "License plate cannot exceed 20 characters")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 200, ErrorMessage = "Capacity must be between 1 and 200")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Bus number is required")]
        [StringLength(10, ErrorMessage = "Bus number cannot exceed 10 characters")]
        public string BusNumber { get; set; } = string.Empty;
    }

    public class UpdateLocationRequest
    {
        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double Longitude { get; set; }

        [Range(0, 200, ErrorMessage = "Speed must be between 0 and 200 km/h")]
        public double? Speed { get; set; }

        [Range(0, 360, ErrorMessage = "Bearing must be between 0 and 360 degrees")]
        public double? Bearing { get; set; }
    }

    public class UpdateStatusRequest
    {
        [Required(ErrorMessage = "Status is required")]
        public BusStatus Status { get; set; }
    }

    public class BusResponse
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string BusNumber { get; set; } = string.Empty;
        public double? CurrentLatitude { get; set; }
        public double? CurrentLongitude { get; set; }
        public double? Speed { get; set; }
        public double? Bearing { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
    }
}