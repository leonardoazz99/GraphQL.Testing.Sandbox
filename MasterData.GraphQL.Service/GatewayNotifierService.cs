namespace MasterData.GraphQL.Service;

/// <summary>
/// Fires once at startup and tells the Fusion gateway to refresh its schema.
/// Retries with exponential back-off so transient gateway unavailability is tolerated.
/// </summary>
public sealed class GatewayNotifierService : BackgroundService
{
    #region Private Fields

    private const int MaxRetries = 5;

    private readonly IHttpClientFactory _http;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GatewayNotifierService> _logger;

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="http"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public GatewayNotifierService(
        IHttpClientFactory http,
        IConfiguration configuration,
        ILogger<GatewayNotifierService> logger)
    {
        _http          = http;
        _configuration = configuration;
        _logger        = logger;
    }

    #endregion

    #region BackgroundService Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var refreshUrl = _configuration["FusionGateway:RefreshUrl"];

        if (string.IsNullOrWhiteSpace(refreshUrl))
        {
            _logger.LogDebug("FusionGateway:RefreshUrl not configured — skipping gateway notification");
            return;
        }

        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

        using var client = _http.CreateClient();

        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                var response = await client.PostAsync(refreshUrl, content: null, stoppingToken)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Gateway schema refresh triggered (attempt {Attempt})", attempt);
                    return;
                }
                _logger.LogWarning("Gateway returned {Status} on refresh attempt {Attempt}",
                    (int)response.StatusCode, attempt);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Could not reach gateway at {Url} (attempt {Attempt}): {Error}",
                    refreshUrl, attempt, ex.Message);
            }

            if (attempt < MaxRetries)
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), stoppingToken);
        }

        _logger.LogError("Failed to notify gateway after 5 attempts — schema may be stale");
    }

    #endregion
}
