using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI.Controllers;

[ApiController]
[Route("api/demo")]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;

    public DemoController(ILogger<DemoController> logger)
    {
        _logger = logger;
    }

    [HttpGet("sync")]
    public IActionResult SyncWait()
    {
        _logger.LogInformation("SYNC: before waiting");
        Thread.Sleep(2000);   // the thread is stuck here, doing nothing
        _logger.LogInformation("SYNC: after waiting");
        return Ok("sync done");
    }

    [HttpGet("async")]
    public async Task<IActionResult> AsyncWait()
    {
        _logger.LogInformation("ASYNC: before waiting");
        await Task.Delay(2000);   // the thread is released here
        _logger.LogInformation("ASYNC: after waiting");
        return Ok("async done");
    }
}