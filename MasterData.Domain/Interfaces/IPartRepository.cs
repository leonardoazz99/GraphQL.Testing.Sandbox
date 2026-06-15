using MasterData.Domain.Entities;

namespace MasterData.Domain.Interfaces;

public interface IPartRepository
{
    Task<Part?> GetAsync(string partNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Part>> GetAllAsync(CancellationToken cancellationToken = default);
}
