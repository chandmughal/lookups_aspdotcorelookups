# ASP.NET Core 10 – Reusable Lookup (LOV) Service

A small MVC example showing how to maintain dropdown/List of Values (LOV) centrally using Dependency Injection.

## What this sample demonstrates

- An `ILookupService` interface and `LookupService` implementation.
- DI registration in `Program.cs`.
- Lookup values returned as a simple `LookupItem` DTO.
- A ViewModel converts lookup DTOs into MVC `SelectListItem` objects.
- A Razor view renders dropdowns with `asp-for` and `asp-items`.
- The POST action reloads dropdown options when validation fails.

## Requirements

- .NET 10 SDK

## Run the project

Open a terminal in the folder containing `LookupsDemo.csproj`, then run:

```powershell
dotnet restore
dotnet run
```

Open the localhost URL printed by the terminal. The default route opens the Course Setup form.

## Where are LOV values maintained?

Edit `Services/LookupService.cs`. Both Course Type and Status lists are maintained centrally and can be reused by other controllers.

## Important

This is intentionally a small demonstration. It does **not** save courses to a database. The lookup service currently returns static values. To use SQL Server or Oracle, keep the same interface and change the service implementation to read lookup records through your data-access layer.

For a larger ERP application, split lookup services by business area (for example, Employee, Inventory, Finance) rather than creating one huge service.
