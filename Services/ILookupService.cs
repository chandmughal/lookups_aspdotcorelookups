namespace LookupsDemo.Services;

public interface ILookupService
{
    IReadOnlyList<LookupItem> GetCourseTypes();
    IReadOnlyList<LookupItem> GetStatuses();
    Task<IReadOnlyList<LookupItem>> GetOracleLovAsync(CancellationToken cancellationToken = default);
}
