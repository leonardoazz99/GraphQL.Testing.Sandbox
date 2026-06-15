namespace ApolloFederation.Service.Options;

public sealed class FusionOptions
{
    public string ArchivePath { get; set; } = "./gateway.far";
    public List<SubgraphOptions> Subgraphs { get; set; } = [];
}
