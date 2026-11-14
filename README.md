# ASP.NET Core 10 – Reusable Lookup (LOV) Service

A small MVC example showing reusable dropdown/List of Values (LOV) data through Dependency Injection. It demonstrates static, Oracle-backed, and SQL Server-backed lookup sources.

## What this sample demonstrates

- An `ILookupService` interface and `LookupService` implementation.
- DI registration with `AddScoped`.
- A small `LookupItem` DTO independent of MVC.
- Static Course Type and Status dropdowns.
- An Oracle-backed dropdown using ODP.NET.
- A SQL Server-backed searchable table LOV using Microsoft.Data.SqlClient.
- Searching across every displayed SQL Server column while submitting only `JobType`.
- Async database access and cancellation tokens.
- A ViewModel carries lookup results to Razor.
- Razor dropdowns use `asp-for` and `asp-items`.

## Requirements

- .NET 10 SDK
- Network access from the application machine to the Oracle and SQL Server database hosts
- Oracle account with SELECT permission on `SHOP.NL_00_01`
- SQL Server account with SELECT permission on `NishatProduction_Intelligence.dbo.DimHeads`

## Configure Oracle connection string securely

In a terminal opened in the folder containing `LookupsDemo.csproj`:

```powershell
dotnet user-secrets set "ConnectionStrings:oradb" "User Id=Shop;Password=YOUR_PASSWORD;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.215)(PORT=1521))(CONNECT_DATA=(SID=orc2)(SERVER=DEDICATED)))"
```

## Configure SQL Server connection string securely

For local development, use User Secrets as well. Replace the example server/database values if needed:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=ImageManagementDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;"
```

The query itself reads from `NishatProduction_Intelligence`. The supplied connection string's initial catalog is `ImageManagementDB`, which is fine because the query uses a three-part database.schema.table name, provided the login has permission to access that database.

For production IIS, configure these keys on the server using protected deployment configuration or environment variables. The environment variable names are `ConnectionStrings__oradb` and `ConnectionStrings__DefaultConnection`. Do not commit passwords to Git.

## Run

```powershell
dotnet restore
dotnet run
```

Open the localhost URL printed by the terminal. The default route opens the Course Setup form.

## Oracle LOV query

```sql
SELECT itm_cod, itm_desc
FROM shop.nl_00_01 d
WHERE cls_typ = 7
ORDER BY 1 DESC;
```

- `ITM_COD` becomes the HTML option value.
- `ITM_DESC` becomes the displayed text.

## SQL Server LOV query

```sql
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
```

All ten columns are displayed and included in the client-side search. When the user selects a row, only `JobType` is assigned to `SelectedJobType` and posted with the form. This sample does not save that value to a database; it demonstrates how to receive the selected key in the controller.

## Code flow

1. `CoursesController` calls the lookup methods.
2. `LookupService` reads the named connection string from configuration.
3. The Oracle method uses ODP.NET; the SQL Server method uses Microsoft.Data.SqlClient.
4. Each method returns lookup data to the controller.
5. The ViewModel carries the rows and selected key to Razor.
6. Razor renders the LOV; the SQL Server table's search filters across all displayed columns.

## Important

This sample does not save the Course form to a database. The Oracle and SQL Server connections are used to populate lookup data only. The SQL Server search currently filters the rows already loaded into the page; for very large tables, implement server-side filtering and paging instead.
