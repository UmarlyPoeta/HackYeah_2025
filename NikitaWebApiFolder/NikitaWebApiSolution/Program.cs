using NikitaWebApiSolution.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка CORS для доступа с других компьютеров
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Добавляем DbContext
builder.Services.AddDbContext<BusDbContext>();

// Слушаем на всех сетевых интерфейсах для внешнего подключения
builder.WebHost.UseUrls("http://*:5041");

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BusDbContext>();

    try
    {
        // УДАЛИТЕ старую базу данных если существует
        // context.Database.EnsureDeleted();

        // Создаем базу данных и таблицы, если их нет
        var created = context.Database.EnsureCreated();
        Console.WriteLine($"Database created: {created}");

        // Проверяем существование таблиц
        var busTableExists = context.Buses.Any();
        var reportTableExists = context.BusReports.Any();

        Console.WriteLine($"Bus table exists: {busTableExists}");
        Console.WriteLine($"BusReports table exists: {reportTableExists}");

        // Если таблица BusReports пустая, добавляем тестовые данные
        if (!context.BusReports.Any())
        {
            Console.WriteLine("Adding test report data...");
            context.BusReports.AddRange(
                new BusReport
                {
                    BusNumber = 101,
                    CrowdingLevel = CrowdingLevel.High,
                    DelayMinutes = 10,
                    VehicleFailure = VehicleFailure.Temporary,
                    AirConditioning = AirConditioning.Good,
                    SmellLevel = SmellLevel.LightSmell,
                    AdditionalComments = "Автобус переполнен в час пик",
                    ReportDate = DateTime.UtcNow.AddHours(-1),
                    UserIP = "127.0.0.1"
                },
                new BusReport
                {
                    BusNumber = 205,
                    CrowdingLevel = CrowdingLevel.Medium,
                    DelayMinutes = 5,
                    VehicleFailure = null,
                    AirConditioning = AirConditioning.Cool,
                    SmellLevel = SmellLevel.Violets,
                    AdditionalComments = "Все в порядке, едет по расписанию",
                    ReportDate = DateTime.UtcNow.AddMinutes(-30),
                    UserIP = "127.0.0.1"
                },
                new BusReport
                {
                    BusNumber = 156,
                    CrowdingLevel = CrowdingLevel.Low,
                    DelayMinutes = 25,
                    VehicleFailure = VehicleFailure.Complete,
                    AirConditioning = AirConditioning.Oven,
                    SmellLevel = SmellLevel.VerySmelly,
                    AdditionalComments = "Автобус сломан, стоит на остановке",
                    ReportDate = DateTime.UtcNow.AddHours(-2),
                    UserIP = "127.0.0.1"
                }
            );
            context.SaveChanges();
            Console.WriteLine("Test report data added successfully");
        }

        Console.WriteLine($"Database initialized: {context.Buses.Count()} buses, {context.BusReports.Count()} reports");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing database: {ex.Message}");
        Console.WriteLine($"Full error: {ex}");
    }
}

// Настройка pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();