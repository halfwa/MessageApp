using MessageApp.API.Hubs;
using MessageApp.API.Middlewares;
using Orleans.Configuration;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

const string AllowedOriginSettings = "AllowedOrigin";
const string invariant = "Npgsql";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
             throw new InvalidOperationException("The GetConnectionString operation returned null");

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

builder.Host.UseOrleans((ctx, siloBuilder) =>
{
    if (builder.Environment.IsDevelopment())
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMemoryGrainStorageAsDefault();
        siloBuilder.AddMemoryGrainStorage("messages");
        siloBuilder.UseInMemoryReminderService();
    }
    else
    {
        siloBuilder.ConfigureEndpoints(11111, 30000);
        siloBuilder.UseAdoNetClustering(options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.UseAdoNetReminderService(options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });
        siloBuilder.AddAdoNetGrainStorage("messages", options =>
        {
            options.Invariant = invariant;
            options.ConnectionString = connectionString;
        });

        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "messageCluster";
            options.ServiceId = "messageService";
        });
    }
});

builder.Services
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
