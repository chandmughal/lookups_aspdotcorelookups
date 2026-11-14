using LookupsDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Scoped is appropriate because the lookup service opens database connections
// per operation and may later depend on scoped data-access services.
builder.Services.AddScoped<ILookupService, LookupService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Courses}/{action=Create}/{id?}");

app.Run();
