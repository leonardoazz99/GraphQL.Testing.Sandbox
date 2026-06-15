using MasterData.Domain.Entities;
using MasterData.Domain.Interfaces;

namespace MasterData.Domain.Managers;

public sealed class PartManager(IPartRepository PartRepository) : IPartManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="partNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Part?> IPartManager.GetAsync(string partNumber, CancellationToken cancellationToken)
        => PartRepository.GetAsync(partNumber, cancellationToken);

    Task<IEnumerable<Part>> IPartManager.GetAllAsync(CancellationToken cancellationToken)
        => PartRepository.GetAllAsync(cancellationToken);
}
