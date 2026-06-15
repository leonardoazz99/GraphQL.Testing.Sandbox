using MasterData.Application.Queries;
using MasterData.Contracts;
using MediatR;

namespace MasterData.GraphQL.Service.Queries;

[QueryType]
public static partial class PartGroupQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<IEnumerable<PartGroupDto>> GetPartGroupsAsync(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => mediator.Send(new GetAllPartGroupsQuery(), cancellationToken);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="mediator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Lookup]
    public static Task<PartGroupDto?> GetPartGroupByCodeAsync(
        string code,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => mediator.Send(new GetPartGroupByCodeQuery(code), cancellationToken);
}
