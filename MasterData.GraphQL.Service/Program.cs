using MasterData.GraphQL.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPartRepository, PartRepository>();

builder
    .AddGraphQL("MasterData")
    .AddServiceTypes();

var app = builder.Build();

app.MapGraphQL();
app.RunWithGraphQLCommands(args); // enables `dotnet run -- schema export`

