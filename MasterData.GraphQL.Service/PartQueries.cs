namespace MasterData.GraphQL.Service;

[QueryType]
public static partial class PartQueries
{
    /// <summary>
    /// All parts. Plain list resolver.
    /// </summary>
    /// <param name="partRepository"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<IEnumerable<Part>> GetPartsAsync(
        [Service] IPartRepository partRepository,
        CancellationToken cancellationToken)
        => partRepository.GetAllAsync(cancellationToken);

    /// <summary>
    /// PUBLIC lookup for a single part by its key (partNumber).
    /// Appears in the composite schema as `partByPartNumber` and is also the
    /// gateway's entry point for resolving a Part by partNumber.
    /// </summary>
    /// <param name="partNumber"></param>
    /// <param name="partRepository"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Lookup]
    public static Task<Part?> GetPartByPartNumberAsync(
        string partNumber,
        [Service] IPartRepository partRepository,
        CancellationToken cancellationToken)
        => partRepository.GetByPartNumberAsync(partNumber, cancellationToken);
}
