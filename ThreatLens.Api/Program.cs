var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logs = new List<SecurityLog>
{
        new SecurityLog
        {
            IpAddress = "192.168.1.20",
            TimeStamp = DateTime.UtcNow,
            EventType = "LOGIN_FAILED",
            UserName = "admin",
            Status = "Failed"
        },

        new SecurityLog
        {
            IpAddress = "10.0.0.5",
            TimeStamp = DateTime.UtcNow,
            EventType = "LOGIN_SUCCESS",
            UserName = "burak",
            Status = "Success"
        },

        // Brute force örneği: aynı IP'den arka arkaya başarısız girişler
        new SecurityLog
        {
            IpAddress = "192.168.1.20",
            TimeStamp = DateTime.UtcNow.AddMinutes(-1),
            EventType = "LOGIN_FAILED",
            UserName = "admin",
            Status = "Failed"
        },

        new SecurityLog
        {
            IpAddress = "192.168.1.20",
            TimeStamp = DateTime.UtcNow.AddMinutes(-2),
            EventType = "LOGIN_FAILED",
            UserName = "root",
            Status = "Failed"
        },

        new SecurityLog
        {
            IpAddress = "192.168.1.20",
            TimeStamp = DateTime.UtcNow.AddMinutes(-3),
            EventType = "LOGIN_FAILED",
            UserName = "administrator",
            Status = "Failed"
        }
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello", () =>
{
    return new
    {
        message = "ThreatLens API is running",
        version = "0.1"
    };
});

app.MapGet("/api/logs", () => logs);

app.MapPost("/api/logs", (SecurityLog newLog) =>
{
    logs.Add(newLog);
    return Results.Created("/api/logs", newLog);
});

// Brute force tespiti: aynı IP'den 3 veya daha fazla başarısız giriş varsa alarm ver
app.MapGet("/api/alerts/bruteforce", () =>
{
    var alerts = logs
        .Where(log => log.EventType == "LOGIN_FAILED")
        .GroupBy(log => log.IpAddress)
        .Where(group => group.Count() >= 3)
        .Select(group => new
        {
            IpAddress = group.Key,
            FailedAttempts = group.Count(),
            TargetUsers = group.Select(log => log.UserName).Distinct(),
            Severity = "High"
        });

    return alerts;
});

app.Run();