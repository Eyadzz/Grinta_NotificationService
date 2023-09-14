using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Consumers;
using NotificationService.Application.Services.Implementation;
using NotificationService.Application.Services.Interfaces;
using NotificationService.Domain;
using NotificationService.Persistence;
using NotificationService.Persistence.Repositories.Implementation;
using NotificationService.Persistence.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Server=localhost;Port=5433;User Id=postgres;Password=postgres;Database=NotificationServiceDB;"));

builder.Services.AddSignalR().AddJsonProtocol(options => { options.PayloadSerializerOptions.PropertyNamingPolicy = null; });

builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING", EnvironmentVariableTarget.Process) ?? "rabbitmq://guest:guest@213.168.249.135:5672/"));
        cfg.ReceiveEndpoint("notifications", e =>
        {
            e.ConfigureConsumer<EmailConsumer>(context);
        });

        cfg.ConcurrentMessageLimit = 1;
    });
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var app = builder.Build();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await context.Database.MigrateAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();