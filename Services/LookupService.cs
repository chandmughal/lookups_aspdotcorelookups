using System.Globalization;
using Oracle.ManagedDataAccess.Client;

namespace LookupsDemo.Services;

public class LookupService : ILookupService
{
    private readonly IConfiguration _configuration;

    public LookupService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

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

    // Reads the third LOV directly from Oracle using ODP.NET.
    public async Task<IReadOnlyList<LookupItem>> GetOracleLovAsync(
        CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("oradb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Oracle connection string 'oradb' is missing. Configure it using .NET User Secrets.");
        }

        const string sql = """
            SELECT itm_cod, itm_desc
            FROM shop.nl_00_01
            WHERE cls_typ = 7
            ORDER BY itm_cod DESC
            """;

        var items = new List<LookupItem>();

        await using var connection = new OracleConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new OracleCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var value = Convert.ToString(reader["ITM_COD"], CultureInfo.InvariantCulture) ?? "";
            var description = reader["ITM_DESC"] == DBNull.Value
                ? ""
                : Convert.ToString(reader["ITM_DESC"], CultureInfo.InvariantCulture) ?? "";

            items.Add(new LookupItem(value, description));
        }

        return items;
    }
}
