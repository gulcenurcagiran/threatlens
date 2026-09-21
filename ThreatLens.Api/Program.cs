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



app.Run();


