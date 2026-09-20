var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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

app.MapGet("/api/logs", () =>
{
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

    return logs;

});




app.Run();


