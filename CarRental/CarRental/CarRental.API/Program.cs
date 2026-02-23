using CarRental.Application.Contracts;
using CarRental.Domain.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Messaging;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton<CarRentalFixture>();

builder.Services.AddControllers().AddJsonOptions(opts =>
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var basePath = AppContext.BaseDirectory;
    foreach (var xmlFile in new[] { "CarRental.API.xml", "CarRental.Application.Contracts.xml", "CarRental.Domain.xml" })
    {
        var path = Path.Combine(basePath, xmlFile);
        if (File.Exists(path))
            c.IncludeXmlComments(path, includeControllerXmlComments: true);
    }
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<Car>,             DbRepository<Car>>();
builder.Services.AddScoped<IRepository<Client>,          DbRepository<Client>>();
builder.Services.AddScoped<IRepository<CarModel>,        DbRepository<CarModel>>();
builder.Services.AddScoped<IRepository<ModelGeneration>, DbRepository<ModelGeneration>>();
builder.Services.AddScoped<IRepository<Rental>,          DbRepository<Rental>>();

// Регистрация RabbitMQ-соединения и фонового потребителя
builder.AddRabbitMQClient("carrental-rabbitmq");
builder.Services.AddHostedService<RentalQueueConsumer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rental API v1"));
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();
app.Run();
