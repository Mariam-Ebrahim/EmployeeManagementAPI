using EmployeeManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmployeeManagementAPI.Controllers;

[ApiController]
[Route("api/demo")]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;

    private readonly AppDbContext _context;

    private readonly IDbContextFactory<AppDbContext> _factory;

    public DemoController(
        ILogger<DemoController> logger,
        AppDbContext context,
        IDbContextFactory<AppDbContext> factory)
    {
        _logger = logger;
        _context = context;
        _factory = factory;
    }
    private static async Task<string> GetEmployeeInfoAsync()
    {
        await Task.Delay(1000);   // pretend: a 1-second external call
        return "employee data";
    }

    private static async Task<string> GetDepartmentInfoAsync()
    {
        await Task.Delay(1000);   // pretend: another 1-second external call
        return "department data";
    }

    [HttpGet("sequential")]
    public async Task<IActionResult> RunSequential()
    {
        var sw = Stopwatch.StartNew();

        var employee = await GetEmployeeInfoAsync();      // wait 1s
        var department = await GetDepartmentInfoAsync();  // then wait 1s more

        return Ok(new { employee, department, ms = sw.ElapsedMilliseconds });
    }

    [HttpGet("parallel")]
    public async Task<IActionResult> RunParallel()
    {
        var sw = Stopwatch.StartNew();

        var employeeTask = GetEmployeeInfoAsync();        // starts, no waiting
        var departmentTask = GetDepartmentInfoAsync();    // starts right away too

        await Task.WhenAll(employeeTask, departmentTask); // wait for both together

        var employee = await employeeTask;                // already finished, instant
        var department = await departmentTask;

        return Ok(new { employee, department, ms = sw.ElapsedMilliseconds });
    }

    [HttpGet("broken-parallel")]
    public async Task<IActionResult> BrokenParallel()
    {
        var listTask = _context.Employees.ToListAsync();
        var countTask = _context.Employees.CountAsync();

        await Task.WhenAll(listTask, countTask);

        return Ok(new { count = await countTask });
    }

    [HttpGet("safe-parallel")]
    public async Task<IActionResult> SafeParallel()
    {
        var listTask = GetEmployeesAsync();
        var countTask = CountEmployeesAsync();

        await Task.WhenAll(listTask, countTask);

        return Ok(new { count = await countTask, employees = await listTask });
    }

    private async Task<List<Models.Employee>> GetEmployeesAsync()
    {
        await using var context = await _factory.CreateDbContextAsync();
        return await context.Employees.ToListAsync();
    }

    private async Task<int> CountEmployeesAsync()
    {
        await using var context = await _factory.CreateDbContextAsync();
        return await context.Employees.CountAsync();
    }
}