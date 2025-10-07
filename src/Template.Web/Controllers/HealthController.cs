using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// アプリケーションのヘルスチェック
    /// </summary>
    /// <returns>ヘルスステータス</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
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
            HealthStatus.Healthy => Ok(response),
            HealthStatus.Degraded => StatusCode(200, response), // 200 but with degraded status
            HealthStatus.Unhealthy => StatusCode(503, response),
            _ => StatusCode(500, response)
        };
    }

    /// <summary>
    /// 簡易ヘルスチェック（ライブネス）
    /// </summary>
    /// <returns>アプリケーションが生きているかどうか</returns>
    [HttpGet("live")]
    public IActionResult Live()
    {
        return Ok(new { Status = "Alive", Timestamp = DateTime.UtcNow });
    }

    /// <summary>
    /// 準備完了チェック（readiness）
    /// </summary>
    /// <returns>アプリケーションがリクエストを受け付ける準備ができているかどうか</returns>
    [HttpGet("ready")]
    public async Task<IActionResult> Ready()
    {
        var healthReport = await _healthCheckService.CheckHealthAsync();
        
        return healthReport.Status switch
        {
            HealthStatus.Healthy => Ok(new { Status = "Ready", Timestamp = DateTime.UtcNow }),
            HealthStatus.Degraded => Ok(new { Status = "Ready (Degraded)", Timestamp = DateTime.UtcNow }),
            HealthStatus.Unhealthy => StatusCode(503, new { Status = "Not Ready", Timestamp = DateTime.UtcNow }),
            _ => StatusCode(500, new { Status = "Unknown", Timestamp = DateTime.UtcNow })
        };
    }
}
