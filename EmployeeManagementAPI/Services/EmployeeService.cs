using EmployeeManagementAPI.Data;
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

    public async Task<List<Employee>> GetAllAsync()
    {
        _logger.LogInformation("Getting all employees");
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting employee {EmployeeId}", id);

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

        return employee ?? throw new NotFoundException($"Employee {id} was not found.");
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created employee {EmployeeId}", employee.Id);
        return employee;
    }
}