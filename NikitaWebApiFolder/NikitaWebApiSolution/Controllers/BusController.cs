using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using NikitaWebApiSolution.Models;

namespace NikitaWebApiSolution.Controllers
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

        // НОВЫЙ МЕТОД: POST api/buses/report - отчет о состоянии автобуса
        [HttpPost("report")]
        public async Task<IActionResult> CreateBusReport([FromBody] CreateBusReportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            // Получаем IP пользователя
            var userIP = HttpContext.Connection.RemoteIpAddress?.ToString();

            var report = new BusReport
            {
                BusNumber = request.BusNumber,
                CrowdingLevel = request.CrowdingLevel,
                DelayMinutes = request.DelayMinutes,
                VehicleFailure = request.VehicleFailure,
                AirConditioning = request.AirConditioning,
                SmellLevel = request.SmellLevel,
                AdditionalComments = request.AdditionalComments,
                ReportDate = DateTime.UtcNow,
                UserIP = userIP
            };

            _context.BusReports.Add(report);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Bus report submitted successfully",
                data = new
                {
                    report.Id,
                    report.BusNumber,
                    CrowdingLevel = report.CrowdingLevel.ToString(),
                    report.DelayMinutes,
                    VehicleFailure = report.VehicleFailure?.ToString(),
                    AirConditioning = report.AirConditioning?.ToString(),
                    SmellLevel = report.SmellLevel?.ToString(),
                    report.ReportDate
                }
            });
        }

        // НОВЫЙ МЕТОД: GET api/buses/reports - получить все отчеты
        [HttpGet("reports")]
        public async Task<IActionResult> GetAllBusReports()
        {
            var reports = await _context.BusReports
                .OrderByDescending(r => r.ReportDate)
                .ToListAsync();

            var result = reports.Select(r => new
            {
                r.Id,
                r.BusNumber,
                CrowdingLevel = r.CrowdingLevel.ToString(),
                r.DelayMinutes,
                VehicleFailure = r.VehicleFailure?.ToString(),
                AirConditioning = r.AirConditioning?.ToString(),
                SmellLevel = r.SmellLevel?.ToString(),
                r.AdditionalComments,
                r.ReportDate,
                r.UserIP
            }).ToList();

            return Ok(new { success = true, data = result });
        }

        // НОВЫЙ МЕТОД: GET api/buses/101/reports - получить отчеты по номеру автобуса
        [HttpGet("{busNumber}/reports")]
        public async Task<IActionResult> GetBusReportsByNumber(int busNumber)
        {
            var reports = await _context.BusReports
                .Where(r => r.BusNumber == busNumber)
                .OrderByDescending(r => r.ReportDate)
                .ToListAsync();

            var result = reports.Select(r => new
            {
                r.Id,
                r.BusNumber,
                CrowdingLevel = r.CrowdingLevel.ToString(),
                r.DelayMinutes,
                VehicleFailure = r.VehicleFailure?.ToString(),
                AirConditioning = r.AirConditioning?.ToString(),
                SmellLevel = r.SmellLevel?.ToString(),
                r.AdditionalComments,
                r.ReportDate,
                r.UserIP
            }).ToList();

            return Ok(new { success = true, data = result });
        }
        // НОВЫЙ МЕТОД: GET api/buses/statistics - получить статистику по отчетам
        [HttpGet("statistics")]
        public async Task<IActionResult> GetBusStatistics()
        {
            try
            {
                var reports = await _context.BusReports.ToListAsync();

                if (!reports.Any())
                {
                    return Ok(new
                    {
                        success = true,
                        message = "No reports available for statistics",
                        data = new object()
                    });
                }

                // Статистика по уровням заполненности
                var crowdingStats = reports
                    .GroupBy(r => r.CrowdingLevel)
                    .Select(g => new
                    {
                        Level = g.Key.ToString(),
                        Count = g.Count(),
                        Percentage = Math.Round((double)g.Count() / reports.Count * 100, 2)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                // Статистика по задержкам
                var delayStats = new
                {
                    AverageDelay = reports.Where(r => r.DelayMinutes.HasValue).Average(r => r.DelayMinutes) ?? 0,
                    MaxDelay = reports.Where(r => r.DelayMinutes.HasValue).Max(r => r.DelayMinutes) ?? 0,
                    MinDelay = reports.Where(r => r.DelayMinutes.HasValue).Min(r => r.DelayMinutes) ?? 0,
                    DelayedBusesCount = reports.Count(r => r.DelayMinutes.HasValue && r.DelayMinutes > 0),
                    OnTimeBusesCount = reports.Count(r => r.DelayMinutes.HasValue && r.DelayMinutes == 0)
                };

                // Статистика по поломкам
                var failureStats = reports
                    .Where(r => r.VehicleFailure.HasValue)
                    .GroupBy(r => r.VehicleFailure)
                    .Select(g => new
                    {
                        FailureType = g.Key?.ToString() ?? "Unknown",
                        Count = g.Count(),
                        Percentage = Math.Round((double)g.Count() / reports.Count(r => r.VehicleFailure.HasValue) * 100, 2)
                    })
                    .ToList();

                // Статистика по кондиционерам
                var acStats = reports
                    .Where(r => r.AirConditioning.HasValue)
                    .GroupBy(r => r.AirConditioning)
                    .Select(g => new
                    {
                        ACStatus = g.Key?.ToString() ?? "Unknown",
                        Count = g.Count(),
                        Percentage = Math.Round((double)g.Count() / reports.Count(r => r.AirConditioning.HasValue) * 100, 2)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                // Статистика по запахам
                var smellStats = reports
                    .Where(r => r.SmellLevel.HasValue)
                    .GroupBy(r => r.SmellLevel)
                    .Select(g => new
                    {
                        SmellLevel = g.Key?.ToString() ?? "Unknown",
                        Count = g.Count(),
                        Percentage = Math.Round((double)g.Count() / reports.Count(r => r.SmellLevel.HasValue) * 100, 2)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                // Статистика по номерам автобусов (топ 10 проблемных)
                var busNumberStats = reports
                    .GroupBy(r => r.BusNumber)
                    .Select(g => new
                    {
                        BusNumber = g.Key,
                        ReportCount = g.Count(),
                        AverageDelay = g.Where(r => r.DelayMinutes.HasValue).Average(r => r.DelayMinutes) ?? 0,
                        MostCommonCrowding = g.GroupBy(r => r.CrowdingLevel)
                                            .OrderByDescending(gr => gr.Count())
                                            .First().Key.ToString(),
                        ProblemScore = CalculateProblemScore(g.ToList())
                    })
                    .OrderByDescending(x => x.ProblemScore)
                    .Take(10)
                    .ToList();

                // Временная статистика (отчеты по часам)
                var hourlyStats = reports
                    .GroupBy(r => r.ReportDate.Hour)
                    .Select(g => new
                    {
                        Hour = g.Key,
                        ReportCount = g.Count(),
                        AverageCrowding = (int)Math.Round(g.Average(r => (int)r.CrowdingLevel))
                    })
                    .OrderBy(x => x.Hour)
                    .ToList();

                var result = new
                {
                    Summary = new
                    {
                        TotalReports = reports.Count,
                        TimeRange = new
                        {
                            OldestReport = reports.Min(r => r.ReportDate),
                            NewestReport = reports.Max(r => r.ReportDate)
                        },
                        ReportsWithComments = reports.Count(r => !string.IsNullOrEmpty(r.AdditionalComments))
                    },
                    CrowdingStatistics = crowdingStats,
                    DelayStatistics = delayStats,
                    FailureStatistics = failureStats,
                    AirConditioningStatistics = acStats,
                    SmellStatistics = smellStats,
                    TopProblematicBuses = busNumberStats,
                    HourlyDistribution = hourlyStats,
                    ProblematicTrends = AnalyzeProblematicTrends(reports)
                };

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error generating statistics", error = ex.Message });
            }
        }
        private static double CalculateProblemScore(List<BusReport> reports)
        {
            double score = 0;

            foreach (var report in reports)
            {
                // Вес факторов
                score += (int)report.CrowdingLevel * 1.5; // Высокая заполненность = больше проблем
                score += (report.DelayMinutes ?? 0) * 0.1; // Задержки увеличивают счет
                score += report.VehicleFailure.HasValue ? 3 : 0; // Поломки серьезно увеличивают счет
                score += report.AirConditioning == AirConditioning.Oven ? 2 : 0; // Плохой кондиционер
                score += (int)(report.SmellLevel ?? 0) * 1.2; // Запахи увеличивают счет
            }

            return Math.Round(score / reports.Count, 2); // Нормализуем по количеству отчетов
        }

        // Вспомогательный метод для анализа трендов проблем
        private static object AnalyzeProblematicTrends(List<BusReport> reports)
        {
            var lastWeekReports = reports.Where(r => r.ReportDate >= DateTime.UtcNow.AddDays(-7)).ToList();

            if (!lastWeekReports.Any())
                return new { Message = "Insufficient data for trend analysis" };

            var dailyStats = lastWeekReports
                .GroupBy(r => r.ReportDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    ReportCount = g.Count(),
                    AverageCrowding = Math.Round(g.Average(r => (int)r.CrowdingLevel), 2),
                    ProblematicReports = g.Count(r =>
                        r.CrowdingLevel >= CrowdingLevel.High ||
                        r.VehicleFailure.HasValue ||
                        r.AirConditioning == AirConditioning.Oven ||
                        (r.SmellLevel ?? 0) >= SmellLevel.Smelly)
                })
                .OrderBy(x => x.Date)
                .ToList();

            var crowdingTrend = dailyStats.Count > 1 ?
                (dailyStats.Last().AverageCrowding - dailyStats.First().AverageCrowding) : 0;

            var problemTrend = dailyStats.Count > 1 ?
                (dailyStats.Last().ProblematicReports - dailyStats.First().ProblematicReports) : 0;

            return new
            {
                AnalysisPeriod = "Last 7 days",
                TotalDays = dailyStats.Count,
                AverageDailyReports = Math.Round(dailyStats.Average(x => x.ReportCount), 1),
                CrowdingTrend = crowdingTrend > 0 ? "Increasing" : crowdingTrend < 0 ? "Decreasing" : "Stable",
                ProblemTrend = problemTrend > 0 ? "Worsening" : problemTrend < 0 ? "Improving" : "Stable",
                WorstDay = dailyStats.OrderByDescending(x => x.ProblematicReports).FirstOrDefault()?.Date,
                DailyBreakdown = dailyStats
            };
        }

        // НОВЫЙ МЕТОД: GET api/buses/statistics/summary - краткая статистика
        [HttpGet("statistics/summary")]
        public async Task<IActionResult> GetStatisticsSummary()
        {
            var reports = await _context.BusReports.ToListAsync();

            if (!reports.Any())
            {
                return Ok(new
                {
                    success = true,
                    message = "No reports available",
                    data = new { TotalReports = 0 }
                });
            }

            var summary = new
            {
                TotalReports = reports.Count,
                MostCommonProblem = GetMostCommonProblem(reports),
                AverageCrowdingLevel = reports.Average(r => (int)r.CrowdingLevel),
                BusesWithProblems = reports.Count(r =>
                    r.CrowdingLevel >= CrowdingLevel.High ||
                    r.VehicleFailure.HasValue ||
                    r.DelayMinutes > 10),
                RecentReports = reports.Count(r => r.ReportDate >= DateTime.UtcNow.AddHours(-24))
            };

            return Ok(new { success = true, data = summary });
        }

        // Вспомогательный метод для определения самой частой проблемы
        private static string GetMostCommonProblem(List<BusReport> reports)
        {
            var problems = new Dictionary<string, int>
            {
                ["High Crowding"] = reports.Count(r => r.CrowdingLevel >= CrowdingLevel.High),
                ["Vehicle Failure"] = reports.Count(r => r.VehicleFailure.HasValue),
                ["Bad AC"] = reports.Count(r => r.AirConditioning == AirConditioning.Oven || r.AirConditioning == AirConditioning.Warm),
                ["Bad Smell"] = reports.Count(r => r.SmellLevel >= SmellLevel.Smelly),
                ["Long Delay"] = reports.Count(r => r.DelayMinutes > 15)
            };

            return problems.OrderByDescending(p => p.Value).First().Key;
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

    // DTO для создания отчета о состоянии автобуса
    public class CreateBusReportRequest
    {
        [Required(ErrorMessage = "Bus number is required")]
        [Range(1, 999, ErrorMessage = "Bus number must be between 1 and 999")]
        public int BusNumber { get; set; }

        [Required(ErrorMessage = "Crowding level is required")]
        public CrowdingLevel CrowdingLevel { get; set; }

        [Range(0, 300, ErrorMessage = "Delay must be between 0 and 300 minutes")]
        public int? DelayMinutes { get; set; }

        public VehicleFailure? VehicleFailure { get; set; }

        public AirConditioning? AirConditioning { get; set; }

        public SmellLevel? SmellLevel { get; set; }

        [StringLength(500, ErrorMessage = "Additional comments cannot exceed 500 characters")]
        public string? AdditionalComments { get; set; }
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