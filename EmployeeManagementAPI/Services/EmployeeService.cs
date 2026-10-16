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
    public async Task UpdateAsync(int id, UpdateEmployeeRequest request)
    {
        var employee = await GetByIdAsync(id); 

        employee.Name = request.Name;
        employee.Department = request.Department;
        employee.Salary = request.Salary;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated employee {EmployeeId}", id);
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await GetByIdAsync(id);   

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted employee {EmployeeId}", id);
    }
}