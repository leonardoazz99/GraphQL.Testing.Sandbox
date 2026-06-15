using MasterData.Domain.Managers;
using MasterData.Infrastructure.Repositories;
using MasterData.Application.Handlers;
using MasterData.Domain.Interfaces;
using MasterData.GraphQL.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHostedService<GatewayNotifierService>();

builder.Services.AddScoped<IPartManager, PartManager>();
builder.Services.AddScoped<IPartGroupManager, PartGroupManager>();

builder.Services.AddScoped<IPartRepository, PartMockRepository>();
builder.Services.AddScoped<IPartGroupRepository, PartGroupMockRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetAllPartsQueryHandler>());

builder
    .AddGraphQL("MasterData")
    .AddServiceTypes();

var app = builder.Build();

app.MapGraphQL();
app.RunWithGraphQLCommands(args);
