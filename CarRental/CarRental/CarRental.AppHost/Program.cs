var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("carrental-sql")
                       .AddDatabase("CarRentalDb");

builder.AddProject<Projects.CarRental_API>("carrental-api")
       .WithReference(sqlServer, "DefaultConnection")
       .WaitFor(sqlServer);

builder.Build().Run();
