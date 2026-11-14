namespace LookupsDemo.Services;

public interface ILookupService
{
    IReadOnlyList<LookupItem> GetCourseTypes();
    IReadOnlyList<LookupItem> GetStatuses();
    Task<IReadOnlyList<LookupItem>> GetOracleLovAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<JobTypeLovItem>> GetSqlServerJobTypesAsync(CancellationToken cancellationToken = default);
}