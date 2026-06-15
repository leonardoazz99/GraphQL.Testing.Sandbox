using MasterData.Contracts;
using MasterData.Application.Queries;
using MediatR;
using MasterData.Domain.Interfaces;

namespace MasterData.Application.Handlers;

public sealed class GetAllPartsQueryHandler(IPartManager PartManager) : IRequestHandler<GetAllPartsQuery, IEnumerable<PartDto>>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task<IEnumerable<PartDto>> IRequestHandler<GetAllPartsQuery, IEnumerable<PartDto>>.Handle(
        GetAllPartsQuery request, 
        CancellationToken cancellationToken)
    {
        var parts = await PartManager.GetAllAsync(cancellationToken).ConfigureAwait(false);
        return parts.Select(p => new PartDto(p.PartNumber, p.Name, p.Material, p.WeightGrams, p.Standard, p.Manufacturer));
    }
}