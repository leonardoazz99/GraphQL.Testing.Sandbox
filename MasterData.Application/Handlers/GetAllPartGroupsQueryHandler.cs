using MasterData.Contracts;
using MasterData.Application.Queries;
using MediatR;
using MasterData.Domain.Interfaces;

namespace MasterData.Application.Handlers;

public sealed class GetAllPartGroupsQueryHandler(IPartGroupManager PartGroupManager) : IRequestHandler<GetAllPartGroupsQuery, IEnumerable<PartGroupDto>>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task<IEnumerable<PartGroupDto>> IRequestHandler<GetAllPartGroupsQuery, IEnumerable<PartGroupDto>>.Handle(
        GetAllPartGroupsQuery request, 
        CancellationToken cancellationToken)
    {
        var groups = await PartGroupManager.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return groups.Select(g => new PartGroupDto(g.Code, g.Name, g.Description));
    }
}
