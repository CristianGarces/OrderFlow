var builder = DistributedApplication.CreateBuilder(args);

// Agregar la conexión a tu PostgreSQL existente
var postgres = builder.AddConnectionString("PostgreSQL");

// Agregar tu proyecto Identity
var identity = builder.AddProject<Projects.Tienda_Identity>("tienda-identity");

builder.Build().Run();
