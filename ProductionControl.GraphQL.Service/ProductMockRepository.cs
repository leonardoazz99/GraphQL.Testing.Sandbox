namespace ProductionControl.GraphQL.Service;

/// <summary>
/// In-memory product data. Note that PN-1001 is referenced by TWO products
/// (a plain and a coated hex bolt). This makes the reverse federation hop
/// (Part -> products) return more than one product, which is nice for testing.
///
/// In a real subgraph, replace this with a DataLoader-backed repository to avoid
/// N+1 lookups during cross-subgraph entity resolution.
/// </summary>
public class ProductMockRepository : IProductRepository
{
    private static readonly List<Product> Products =
    [
        new Product { Id = 1, Name = "Hex Bolt M8",          Category = "Fasteners", PartNumber = "PN-1001", Price = 0.45m },
        new Product { Id = 2, Name = "Steel Bracket 90\u00B0", Category = "Brackets",  PartNumber = "PN-1002", Price = 3.20m },
        new Product { Id = 3, Name = "Rubber Gasket 50mm",    Category = "Seals",     PartNumber = "PN-1003", Price = 1.10m },
        new Product { Id = 4, Name = "Hex Bolt M8 (Coated)",  Category = "Fasteners", PartNumber = "PN-1001", Price = 0.55m },
    ];

    Task<Product?> IProductRepository.GetByIdAsync(int id, CancellationToken cancellationToken)
        => Task.FromResult(Products.FirstOrDefault(p => p.Id == id));

    Task<IEnumerable<Product>> IProductRepository.GetByPartNumberAsync(string partNumber, CancellationToken cancellationToken)
        => Task.FromResult(Products.Where(p => p.PartNumber == partNumber));

    Task<IEnumerable<Product>> IProductRepository.GetAllAsync(CancellationToken cancellationToken) => Task.FromResult((IEnumerable<Product>)Products);
}
