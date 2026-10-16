using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Exceptions;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;

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

    public List<Employee> GetAll()
    {
        _logger.LogInformation("Getting all employees");
        return _context.Employees.ToList();
    }

    public Employee GetById(int id)
    {
        _logger.LogInformation("Getting employee {EmployeeId}", id);

        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);

        return employee ?? throw new NotFoundException($"Employee {id} was not found.");
    }

    public Employee Create(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        _logger.LogInformation("Created employee {EmployeeId}", employee.Id);
        return employee;
    }
}