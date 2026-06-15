namespace MasterData.GraphQL.Service;

public interface IPartRepository
{

    Task<Part?> GetByPartNumberAsync(string partNumber, CancellationToken cancellationToken = default);

    Task<IEnumerable<Part>> GetAllAsync(CancellationToken cancellationToken = default);
}
