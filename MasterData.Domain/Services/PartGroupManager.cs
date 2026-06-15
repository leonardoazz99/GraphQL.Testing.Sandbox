using MasterData.Domain.Entities;
using MasterData.Domain.Interfaces;

namespace MasterData.Domain.Managers;

public sealed class PartGroupManager(IPartGroupRepository PartGroupRepository) : IPartGroupManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PartGroup?> IPartGroupManager.GetByCodeAsync(string code, CancellationToken cancellationToken)
        => PartGroupRepository.GetByCodeAsync(code, cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<PartGroup>> IPartGroupManager.GetAllAsync(CancellationToken cancellationToken)
        => PartGroupRepository.GetAllAsync(cancellationToken);
}
