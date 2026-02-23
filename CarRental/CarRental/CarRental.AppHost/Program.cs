var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("carrental-sql")
                       .AddDatabase("CarRentalDb");

// RabbitMQ с панелью управления (Management UI на порту 15672)
var rabbitMq = builder.AddRabbitMQ("carrental-rabbitmq")
                      .WithManagementPlugin();

builder.AddProject<Projects.CarRental_API>("carrental-api")
       .WithReference(sqlServer, "DefaultConnection")
       .WithReference(rabbitMq)
       .WithEnvironment("RabbitMQ__ExchangeName", "rental-exchange")
       .WithEnvironment("RabbitMQ__QueueName",    "rental-queue")
       .WaitFor(sqlServer)
       .WaitFor(rabbitMq);

builder.AddProject<Projects.CarRental_Generator_Host>("carrental-generator")
       .WithReference(rabbitMq)
       .WithEnvironment("RabbitMQ__ExchangeName", "rental-exchange")
       .WaitFor(rabbitMq);

builder.Build().Run();
