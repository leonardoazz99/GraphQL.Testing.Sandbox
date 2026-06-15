using MasterData.Application.Queries;
using MasterData.Contracts;
using MediatR;

namespace MasterData.GraphQL.Service.Queries;

/// <summary>
/// Product is owned by ProductionControl. MasterData contributes a single field: <c>part</c>.
/// This record is a federation stub — the gateway resolves it using partNumber as the key.
/// </summary>
public sealed record Product(string PartNumber)
{
    public Task<PartDto?> GetPartAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => mediator.Send(new GetPartQuery(PartNumber), cancellationToken);
}

[QueryType]
public static partial class ProductQueries
{
    [Lookup, Internal]
    public static Product? GetProductByPartNumber(string partNumber)
        => new(partNumber);
}
