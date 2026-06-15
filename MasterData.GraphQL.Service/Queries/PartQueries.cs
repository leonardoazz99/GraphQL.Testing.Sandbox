using MasterData.Application.Queries;
using MasterData.Contracts;
using MediatR;

namespace MasterData.GraphQL.Service.Queries;

[QueryType]
public static partial class PartQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<IEnumerable<PartDto>> GetPartsAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => mediator.Send(new GetAllPartsQuery(), cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="partNumber"></param>
    /// <param name="mediator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Lookup]
    public static Task<PartDto?> GetPartAsync(
        string partNumber,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => mediator.Send(new GetPartQuery(partNumber), cancellationToken);
}
