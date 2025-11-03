using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Template.Functions.Functions;

public class HealthFunctions
{
    private readonly HealthCheckService _healthCheckService;
    private readonly ILogger<HealthFunctions> _logger;

    public HealthFunctions(HealthCheckService healthCheckService, ILogger<HealthFunctions> logger)
    {
        _healthCheckService = healthCheckService;
        _logger = logger;
    }

    /// <summary>
    /// アプリケーションのヘルスチェック
    /// </summary>
    [Function("Health")]
    public async Task<IActionResult> GetHealth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/health")] HttpRequest req)
    {
        var healthReport = await _healthCheckService.CheckHealthAsync();
        
        var status = healthReport.Status switch
        {
            HealthStatus.Healthy => "Healthy",
            HealthStatus.Degraded => "Degraded",
            HealthStatus.Unhealthy => "Unhealthy",
            _ => "Unknown"
        };

        var response = new
        {
            Status = status,
            TotalDuration = healthReport.TotalDuration,
            Checks = healthReport.Entries.Select(entry => new
            {
                Name = entry.Key,
                Status = entry.Value.Status.ToString(),
                Duration = entry.Value.Duration,
                Description = entry.Value.Description,
                Exception = entry.Value.Exception?.Message
            })
        };

        return healthReport.Status switch
        {
            HealthStatus.Healthy => new OkObjectResult(response),
            HealthStatus.Degraded => new ObjectResult(response) { StatusCode = 200 }, // 200 but with degraded status
            HealthStatus.Unhealthy => new ObjectResult(response) { StatusCode = 503 },
            _ => new ObjectResult(response) { StatusCode = 500 }
        };
    }

    /// <summary>
    /// 簡易ヘルスチェック（ライブネス）
    /// </summary>
    [Function("HealthLive")]
    public IActionResult GetLive(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/health/live")] HttpRequest req)
    {
        return new OkObjectResult(new { Status = "Alive", Timestamp = DateTime.UtcNow });
    }

    /// <summary>
    /// 準備完了チェック（readiness）
    /// </summary>
    [Function("HealthReady")]
    public async Task<IActionResult> GetReady(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "api/health/ready")] HttpRequest req)
    {
        var healthReport = await _healthCheckService.CheckHealthAsync();
        
        return healthReport.Status switch
        {
            HealthStatus.Healthy => new OkObjectResult(new { Status = "Ready", Timestamp = DateTime.UtcNow }),
            HealthStatus.Degraded => new OkObjectResult(new { Status = "Ready (Degraded)", Timestamp = DateTime.UtcNow }),
            HealthStatus.Unhealthy => new ObjectResult(new { Status = "Not Ready", Timestamp = DateTime.UtcNow }) { StatusCode = 503 },
            _ => new ObjectResult(new { Status = "Unknown", Timestamp = DateTime.UtcNow }) { StatusCode = 500 }
        };
    }
}

