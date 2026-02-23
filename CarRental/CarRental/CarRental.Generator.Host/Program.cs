using CarRental.Generator.Host.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Регистрация RabbitMQ-соединения через Aspire
builder.AddRabbitMQClient("carrental-rabbitmq");

builder.Services.AddScoped<RentalPublisher>();

// HTTP-клиент для запросов к CarRental API (Aspire service discovery)
builder.Services.AddHttpClient("carrental-api", c =>
{
    c.BaseAddress = new Uri("https+http://carrental-api");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.GetName().Name?.StartsWith("CarRental") == true);

    foreach (var asm in assemblies)
    {
        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{asm.GetName().Name}.xml");
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
