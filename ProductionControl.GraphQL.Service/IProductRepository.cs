namespace ProductionControl.GraphQL.Service;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> GetByPartNumberAsync(string partNumber, CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
}
