var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Tienda_Identity>("tienda-identity");

builder.Build().Run();
