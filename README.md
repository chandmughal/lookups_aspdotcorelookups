# ASP.NET Core 10 – Reusable Lookup (LOV) Service

A small MVC example showing reusable dropdown/List of Values (LOV) data through Dependency Injection. It demonstrates both static lookup lists and a database-driven Oracle lookup.

## What this sample demonstrates

- An `ILookupService` interface and `LookupService` implementation.
- DI registration with `AddScoped`.
- A small `LookupItem` DTO independent of MVC.
- Static Course Type and Status dropdowns.
- An Oracle-backed dropdown using ODP.NET.
- Async database access and cancellation tokens.
- A ViewModel converts lookup DTOs into MVC `SelectListItem` objects.
- Razor dropdowns use `asp-for` and `asp-items`.

## Requirements

- .NET 10 SDK
- Network access from the application machine to the Oracle database host and port
- Oracle account with SELECT permission on `SHOP.NL_00_01`

## Configure the Oracle connection string securely

Do not commit database passwords to Git. In a terminal opened in the folder containing `LookupsDemo.csproj`, store the connection string locally (replace the password placeholder with the database password):

```powershell
dotnet user-secrets set "ConnectionStrings:oradb" "User Id=Shop;Password=YOUR_PASSWORD;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.215)(PORT=1521))(CONNECT_DATA=(SID=orc2)(SERVER=DEDICATED)))"
```

User Secrets override appsettings configuration in Development and are not committed to the repository.

## Run

```powershell
dotnet restore
dotnet run
```

Open the localhost URL printed by the terminal. The default route opens the Course Setup form.

## Oracle LOV query

The Oracle dropdown executes:

```sql
SELECT itm_cod, itm_desc
FROM shop.nl_00_01 d
WHERE cls_typ = 7
ORDER BY 1 DESC;
```

- `ITM_COD` becomes the HTML option value.
- `ITM_DESC` becomes the text displayed to the user.

The query selects only the two columns needed for the dropdown; it does not need `ROWID` or `SELECT d.*`.

## Code flow

1. `CoursesController` calls `GetOracleLovAsync()`.
2. `LookupService` reads `ConnectionStrings:oradb` from configuration.
3. ODP.NET opens an Oracle connection and executes the query asynchronously.
4. The service returns `LookupItem` records.
5. The controller maps them to `SelectListItem`.
6. Razor renders the dropdown.

## Important

This sample does not save the Course form to a database. The Oracle connection is used only to populate the third dropdown. The service uses ODP.NET directly, not Entity Framework Core.

If Oracle 11g connectivity fails with the selected ODP.NET Core package, check the provider's supported database versions. An older Oracle database may require a compatible provider/driver version or a different data-access approach.
