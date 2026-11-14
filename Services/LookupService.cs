using System.Globalization;
using LookupsDemo.Models;
using Microsoft.Data.SqlClient;
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

    // Reads the Oracle LOV using ODP.NET.
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
            FROM shop.nl_00_01 d
            WHERE cls_typ = 7
            ORDER BY 1 DESC
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

    // Reads all displayed columns from SQL Server. JobType is the selected key;
    // the other columns are shown to help the user identify the correct row.
    public async Task<IReadOnlyList<JobTypeLovItem>> GetSqlServerJobTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "SQL Server connection string 'DefaultConnection' is missing. Configure it using .NET User Secrets.");
        }

        const string sql = """
            SELECT
                [JobType],
                [JobDescription],
                [MachineType],
                [CategoryHead],
                [Concern],
                [TypeStatus],
                [CuttingDepartment],
                [StitchingDepartment],
                [InspectionDepartment],
                [PackingDepartment]
            FROM [NishatProduction_Intelligence].[dbo].[DimHeads]
            ORDER BY [JobType];
            """;

        var items = new List<JobTypeLovItem>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            string ReadText(string columnName) =>
                reader[columnName] == DBNull.Value
                    ? ""
                    : Convert.ToString(reader[columnName], CultureInfo.InvariantCulture) ?? "";

            items.Add(new JobTypeLovItem
            {
                JobType = ReadText("JobType"),
                JobDescription = ReadText("JobDescription"),
                MachineType = ReadText("MachineType"),
                CategoryHead = ReadText("CategoryHead"),
                Concern = ReadText("Concern"),
                TypeStatus = ReadText("TypeStatus"),
                CuttingDepartment = ReadText("CuttingDepartment"),
                StitchingDepartment = ReadText("StitchingDepartment"),
                InspectionDepartment = ReadText("InspectionDepartment"),
                PackingDepartment = ReadText("PackingDepartment")
            });
        }

        return items;
    }
}