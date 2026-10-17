namespace EmployeeManagementAPI.Dtos;

public record PerformanceComparisonResponse(
    double SequentialMs,
    double ParallelMs,
    double SavedMs,
    double ImprovementPercent);