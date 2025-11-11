using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Para la base de datos postgres
var postgres = builder.AddPostgres("postgres").WithLifetime(ContainerLifetime.Persistent);
var postgresdb = postgres.AddDatabase("identityDb");

// Agregar tu proyecto Identity
var identity = builder.AddProject<Projects.Tienda_Identity>("tienda-identity").WithReference(postgresdb);

var frontend = builder.AddNpmApp("frontend-react", "../Tienda.React", "dev")
    .WithReference(identity);

builder.Build().Run();
