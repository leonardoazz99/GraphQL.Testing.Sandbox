namespace ProductionControl.GraphQL.Service;

/// <summary>
/// Fires once at startup and tells the Fusion gateway to refresh its schema.
/// Retries with exponential back-off so transient gateway unavailability is tolerated.
/// </summary>
public sealed class GatewayNotifierService : BackgroundService
{
    private readonly IHttpClientFactory _http;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GatewayNotifierService> _logger;

    public GatewayNotifierService(
        IHttpClientFactory http,
        IConfiguration configuration,
        ILogger<GatewayNotifierService> logger)
    {
        _http          = http;
        _configuration = configuration;
        _logger        = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var refreshUrl = _configuration["FusionGateway:RefreshUrl"];
        if (string.IsNullOrWhiteSpace(refreshUrl))
        {
            _logger.LogDebug("FusionGateway:RefreshUrl not configured — skipping gateway notification");
            return;
        }

        // Give the host a moment to finish binding its HTTP port before
        // asking the gateway to pull our SDL.
        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);

        using var client = _http.CreateClient();

        for (int attempt = 1; attempt <= 5; attempt++)
        {
            try
            {
                var response = await client.PostAsync(refreshUrl, content: null, stoppingToken);
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

            if (attempt < 5)
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), stoppingToken);
        }

        _logger.LogError("Failed to notify gateway after 5 attempts — schema may be stale");
    }
}
