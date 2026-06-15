using System.Diagnostics;
using ApolloFederation.Service.Options;
using Microsoft.Extensions.Options;

namespace ApolloFederation.Service;

/// <summary>
/// Fetches the SDL from every configured subgraph, runs nitro fusion compose,
/// and overwrites gateway.far — the FileSystemWatcher inside
/// FileSystemFusionConfigurationProvider picks up the change and the gateway
/// hot-reloads without a restart.
/// </summary>
public sealed class SchemaRefreshService
{
    #region Private Fields

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly FusionOptions _options;
    private readonly ILogger<SchemaRefreshService> _logger;

    // Only one compose at a time — concurrent deploys queue up and the last one wins.
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    public SchemaRefreshService(
        IHttpClientFactory httpClientFactory,
        IOptions<FusionOptions> options,
        ILogger<SchemaRefreshService> logger)
    {
        _httpClientFactory  = httpClientFactory;
        _options            = options.Value;
        _logger             = logger;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<RefreshResult> RefreshAsync(CancellationToken ct = default)
    {
        // Non-blocking: if a compose is already running, return immediately.
        if (!await _semaphore.WaitAsync(TimeSpan.Zero, ct))
        {
            _logger.LogInformation("Schema refresh already in progress — skipping duplicate request");
            return RefreshResult.Busy;
        }

        try
        {
            return await ComposeAsync(ct).ConfigureAwait(false);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    #endregion

    #region Private Fields

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<RefreshResult> ComposeAsync(CancellationToken ct)
    {
        var http      = _httpClientFactory.CreateClient("fusion");
        var composeDir = Path.Combine(Path.GetTempPath(), "fusion-compose");
        var schemaArgs = new List<string>();

        foreach (var subgraph in _options.Subgraphs)
        {
            string sdl;
            try
            {
                sdl = await http.GetStringAsync($"{subgraph.Url.TrimEnd('/')}?sdl", ct);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Could not reach subgraph {Name} at {Url}: {Error}",
                    subgraph.Name, subgraph.Url, ex.Message);
                return RefreshResult.Fail($"Subgraph '{subgraph.Name}' is unreachable");
            }

            var subDir = Path.Combine(composeDir, subgraph.Name);
            Directory.CreateDirectory(subDir);

            var schemaFile   = Path.Combine(subDir, "schema.graphqls");
            var settingsFile = Path.Combine(subDir, "schema-settings.json");

            var schemaFileTask = File.WriteAllTextAsync(schemaFile, sdl, ct);
            var settingsFileTask = File.WriteAllTextAsync(settingsFile,
                $$$$"""{"name":"{{{{subgraph.Name}}}}","transports":{"http":{"url":"{{{{subgraph.Url}}}}"}}}""",
                ct);

            await Task.WhenAll(schemaFileTask, settingsFileTask).ConfigureAwait(false);
            schemaArgs.Add($"-f \"{schemaFile}\"");

            _logger.LogDebug("Fetched SDL for {Name} ({Length} chars)", subgraph.Name, sdl.Length);
        }

        var archivePath = Path.GetFullPath(_options.ArchivePath);
        var nitroArgs   = $"fusion compose {string.Join(" ", schemaArgs)} -a \"{archivePath}\"";

        _logger.LogInformation("Composing gateway schema: nitro {Args}", nitroArgs);

        var psi = new ProcessStartInfo("nitro", nitroArgs)
        {
            RedirectStandardOutput = true,
            RedirectStandardError  = true,
            UseShellExecute        = false,
            CreateNoWindow         = true,
        };

        using var process = Process.Start(psi)
            ?? throw new InvalidOperationException("Failed to start nitro process");

        var stderr = await process.StandardError.ReadToEndAsync(ct);
        await process.WaitForExitAsync(ct);

        if (process.ExitCode != 0)
        {
            _logger.LogError("nitro compose failed (exit {Code}): {Error}", process.ExitCode, stderr);
            return RefreshResult.Fail($"Compose failed: {stderr.Trim()}");
        }

        // ── 3. gateway.far has been overwritten ──────────────────────────────────
        // FileSystemFusionConfigurationProvider's internal FileSystemWatcher
        // detects the change and the gateway reloads the schema automatically.
        _logger.LogInformation("Gateway schema refreshed → {Archive}", archivePath);
        return RefreshResult.Ok;
    }

    #endregion
}

public sealed record RefreshResult(bool IsSuccess, string? Error)
{
    public static readonly RefreshResult Ok   = new(true,  null);
    public static readonly RefreshResult Busy = new(false, "A schema refresh is already in progress");

    public static RefreshResult Fail(string reason) => new(false, reason);
}
