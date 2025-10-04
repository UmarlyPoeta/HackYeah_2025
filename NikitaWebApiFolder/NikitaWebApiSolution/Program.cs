using BusTrackingApp.Controllers;

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
        // Создаем базу данных и таблицы, если их нет
        context.Database.EnsureCreated();
        Console.WriteLine("Database created successfully");

        // Добавляем тестовые данные, если таблица пустая
        if (!context.Buses.Any())
        {
            context.Buses.AddRange(
                new Bus
                {
                    LicensePlate = "А123АА777",
                    Model = "PAZ-3205",
                    Capacity = 45,
                    BusNumber = "101",
                    CurrentLatitude = 55.7558,
                    CurrentLongitude = 37.6173,
                    Status = BusStatus.Active,
                    LastUpdate = DateTime.UtcNow
                },
                new Bus
                {
                    LicensePlate = "В456ВВ777",
                    Model = "LiAZ-5292",
                    Capacity = 85,
                    BusNumber = "205",
                    CurrentLatitude = 55.7517,
                    CurrentLongitude = 37.6178,
                    Status = BusStatus.Active,
                    LastUpdate = DateTime.UtcNow
                },
                new Bus
                {
                    LicensePlate = "С789СС777",
                    Model = "MAZ-203",
                    Capacity = 90,
                    BusNumber = "156",
                    Status = BusStatus.Maintenance,
                    LastUpdate = DateTime.UtcNow.AddHours(-2)
                }
            );
            context.SaveChanges();
            Console.WriteLine("Test data added successfully");
        }
        else
        {
            Console.WriteLine("Database already contains data");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing database: {ex.Message}");
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

// Выводим информацию о доступных адресах
Console.WriteLine("Application is running on:");
Console.WriteLine("Local: http://localhost:5041");
Console.WriteLine("Network: http://[your-ip-address]:5041");
Console.WriteLine("Swagger: http://localhost:5041/swagger");

app.Run();