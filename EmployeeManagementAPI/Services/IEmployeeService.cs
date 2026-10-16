using EmployeeManagementAPI.Dtos;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task UpdateAsync(int id, UpdateEmployeeRequest request);
    Task DeleteAsync(int id);
}