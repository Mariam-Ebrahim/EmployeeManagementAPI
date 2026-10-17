using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementAPI.Controllers;

[ApiController]
[Route("api/performance")]
public class PerformanceController : ControllerBase
{
    private readonly IPerformanceService _service;

    public PerformanceController(IPerformanceService service)
    {
        _service = service;
    }

    [HttpGet("compare")]
    public async Task<IActionResult> Compare(
        [Range(0, 10000)] int externalDelayMs = 1000,
        CancellationToken cancellationToken = default)
        => Ok(await _service.CompareAsync(externalDelayMs, cancellationToken));
}