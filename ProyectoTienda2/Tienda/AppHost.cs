var builder = DistributedApplication.CreateBuilder(args);

// Para la base de datos postgres
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHostPort(49729);
var postgresdb = postgres.AddDatabase("identityDb");

// Agregar tu proyecto Identity
var identity = builder.AddProject<Projects.Tienda_Identity>("tienda-identity").WithReference(postgresdb).WaitFor(postgres);

var frontend = builder.AddNpmApp("frontend-react", "../Tienda.React", "dev")
    .WithReference(identity)
    .WithHttpEndpoint(port: 5173, env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
