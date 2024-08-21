using MessageApp.API;
using MessageApp.API.Hubs;
using MessageApp.API.Middlewares;
using MessageApp.BLL.Interfaces;
using MessageApp.BLL.Services;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

const string AllowedOriginSettings = "AllowedOrigin";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

// Swagger/OpenAPI https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder
    .AddDataLayerAccess();
builder.Services
    .AddTransient<IMessageService, MessagesService>()
    .AddHttpContextAccessor()
    .AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(corsBuilder =>
    {
        corsBuilder.WithOrigins(builder.Configuration[AllowedOriginSettings]!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.MapHub<MessageHub>("api/messageHub");
app.Run();
