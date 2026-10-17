using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Dtos;
using EmployeeManagementAPI.Exceptions;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(AppDbContext context, ILogger<EmployeeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all employees");
        return _context.Employees.ToListAsync(cancellationToken);
    }

    public async Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting employee {EmployeeId}", id);

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return employee ?? throw new NotFoundException($"Employee {id} was not found.");
    }

    public async Task<Employee> CreateAsync(Employee employee, CancellationToken cancellationToken)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created employee {EmployeeId}", employee.Id);
        return employee;
    }

    public async Task UpdateAsync(int id, UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await GetByIdAsync(id, cancellationToken);

        employee.Name = request.Name;
        employee.Department = request.Department;
        employee.Salary = request.Salary;

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Updated employee {EmployeeId}", id);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var employee = await GetByIdAsync(id, cancellationToken);

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Deleted employee {EmployeeId}", id);
    }
}