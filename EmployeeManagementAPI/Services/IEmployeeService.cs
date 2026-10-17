using EmployeeManagementAPI.Dtos;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken);
    Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Employee> CreateAsync(Employee employee, CancellationToken cancellationToken);
    Task UpdateAsync(int id, UpdateEmployeeRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}