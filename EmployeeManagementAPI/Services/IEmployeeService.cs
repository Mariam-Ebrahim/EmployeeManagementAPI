using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services;

public interface IEmployeeService
{
    List<Employee> GetAll();
    Employee GetById(int id);
    Employee Create(Employee employee);
}