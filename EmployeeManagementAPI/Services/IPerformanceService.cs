using EmployeeManagementAPI.Dtos;

namespace EmployeeManagementAPI.Services;

public interface IPerformanceService
{
    Task<PerformanceComparisonResponse> CompareAsync(int externalDelayMs, CancellationToken cancellationToken);
}