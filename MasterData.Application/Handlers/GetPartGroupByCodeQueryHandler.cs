using MasterData.Contracts;
using MasterData.Application.Queries;
using MediatR;
using MasterData.Domain.Interfaces;

namespace MasterData.Application.Handlers;

public sealed class GetPartGroupByCodeQueryHandler(IPartGroupManager PartGroupManager) : IRequestHandler<GetPartGroupByCodeQuery, PartGroupDto?>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task<PartGroupDto?> IRequestHandler<GetPartGroupByCodeQuery, PartGroupDto?>.Handle(GetPartGroupByCodeQuery request, CancellationToken cancellationToken)
    {
        var partGroup = await PartGroupManager.GetByCodeAsync(request.Code, cancellationToken).ConfigureAwait(false);
        if (partGroup is null) return null;
        return new PartGroupDto(partGroup.Code, partGroup.Name, partGroup.Description);
    }
}
