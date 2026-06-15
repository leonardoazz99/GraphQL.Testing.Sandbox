using ProductionControl.GraphQL.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, ProductMockRepository>();

builder
    .AddGraphQL("ProductionControl")
    .AddServiceTypes();

var app = builder.Build();

app.MapGraphQL();
app.RunWithGraphQLCommands(args); // enables `dotnet run -- schema export`
