using ApolloFederation.Service;
using ApolloFederation.Service.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("fusion");

builder.Services.Configure<FusionOptions>(builder.Configuration.GetSection("Fusion"));
builder.Services.AddSingleton<SchemaRefreshService>();

builder
    .AddGraphQLGateway()
    .AddFileSystemConfiguration("./gateway.far")
    .ModifyRequestOptions(o => o.CollectOperationPlanTelemetry = true);

var app = builder.Build();

app.MapGraphQL();

app.MapPost("/schema/refresh", async (SchemaRefreshService refresh, CancellationToken ct) =>
{
    var result = await refresh.RefreshAsync(ct);

    return result.IsSuccess
        ? Results.Ok(new { message = "Schema refreshed successfully" })
        : Results.Accepted(value: new { message = result.Error });
});

app.Run();
