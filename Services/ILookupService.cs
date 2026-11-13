namespace LookupsDemo.Services;

public interface ILookupService
{
    IReadOnlyList<LookupItem> GetCourseTypes();
    IReadOnlyList<LookupItem> GetStatuses();
}
