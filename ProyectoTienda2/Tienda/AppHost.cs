using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Para la base de datos
var postgres = builder.AddPostgres("postgres");
var postgresdb = postgres.AddDatabase("identityDb");

// Agregar tu proyecto Identity
var identity = builder.AddProject<Projects.Tienda_Identity>("tienda-identity").WithReference(postgresdb);

builder.Build().Run();
