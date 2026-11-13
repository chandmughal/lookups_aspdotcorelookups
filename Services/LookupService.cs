namespace LookupsDemo.Services;

public class LookupService : ILookupService
{
    // Keep centrally maintained values here for this demonstration.
    // Later these methods can read the values from SQL Server or Oracle.
    public IReadOnlyList<LookupItem> GetCourseTypes() =>
    [
        new("CERT", "Certificate"),
        new("DIP", "Diploma"),
        new("DEG", "Degree")
    ];

    public IReadOnlyList<LookupItem> GetStatuses() =>
    [
        new("1", "Active"),
        new("0", "Inactive")
    ];
}
