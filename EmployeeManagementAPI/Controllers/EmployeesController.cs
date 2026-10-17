using EmployeeManagementAPI.Dtos;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _service.GetAllAsync(cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        => Ok(await _service.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(employee, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        await _service.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}