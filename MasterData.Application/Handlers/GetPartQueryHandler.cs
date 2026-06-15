using MasterData.Contracts;
using MasterData.Application.Queries;
using MediatR;
using MasterData.Domain.Interfaces;

namespace MasterData.Application.Handlers;

public sealed class GetPartQueryHandler(IPartManager PartManager) : IRequestHandler<GetPartQuery, PartDto?>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task<PartDto?> IRequestHandler<GetPartQuery, PartDto?>.Handle(
        GetPartQuery request, 
        CancellationToken cancellationToken)
    {
        var part = await PartManager.GetAsync(request.PartNumber, cancellationToken).ConfigureAwait(false);
        if (part is null) return null;
        return new PartDto(part.PartNumber, part.Name, part.Material, part.WeightGrams, part.Standard, part.Manufacturer);
    }
}
