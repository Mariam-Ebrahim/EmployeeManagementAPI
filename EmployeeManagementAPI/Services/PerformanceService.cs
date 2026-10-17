using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Dtos;
using EmployeeManagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmployeeManagementAPI.Services;

public class PerformanceService : IPerformanceService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public PerformanceService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<PerformanceComparisonResponse> CompareAsync(
        int externalDelayMs, CancellationToken cancellationToken)
    {
        // Warm-up: the very first EF query is slower
        await GetEmployeeCountAsync(0,cancellationToken);

        var sequentialMs = await MeasureSequentialAsync(externalDelayMs, cancellationToken);
        var parallelMs = await MeasureParallelAsync(externalDelayMs, cancellationToken);

        var savedMs = sequentialMs - parallelMs;
        var improvement = sequentialMs == 0 ? 0 : savedMs * 100 / sequentialMs;

        return new PerformanceComparisonResponse(
            Math.Round(sequentialMs, 2),
            Math.Round(parallelMs, 2),
            Math.Round(savedMs, 2),
            Math.Round(improvement, 1));
    }

    private async Task<double> MeasureSequentialAsync(int delayMs, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        await GetEmployeeCountAsync(delayMs, cancellationToken);              
        await SimulateExternalServiceAsync(delayMs, cancellationToken); 

        stopwatch.Stop();
        return stopwatch.Elapsed.TotalMilliseconds;
    }

    private async Task<double> MeasureParallelAsync(int delayMs, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var employeesTask = GetEmployeeCountAsync(delayMs, cancellationToken);             
        var externalTask = SimulateExternalServiceAsync(delayMs, cancellationToken); 

        await Task.WhenAll(employeesTask, externalTask);

        stopwatch.Stop();
        return stopwatch.Elapsed.TotalMilliseconds;
    }

    private async Task<int> GetEmployeeCountAsync(int delayMs, CancellationToken cancellationToken)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);

        var employees = await context.Employees.AsNoTracking().ToListAsync(cancellationToken);

        await Task.Delay(delayMs, cancellationToken);   // pretend this database query is slow

        return employees.Count;
    }

    private static Task SimulateExternalServiceAsync(int delayMs, CancellationToken cancellationToken)
        => Task.Delay(delayMs, cancellationToken);
}