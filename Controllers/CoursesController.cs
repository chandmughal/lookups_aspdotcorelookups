using LookupsDemo.Services;
using LookupsDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LookupsDemo.Controllers;

public class CoursesController : Controller
{
    private readonly ILookupService _lookupService;

    public CoursesController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var model = new CourseViewModel();
        await LoadLookupsAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CourseViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadLookupsAsync(model, cancellationToken);
            return View(model);
        }

        // This demo intentionally does not save the course to a database.
        // SelectedJobType contains the JobType key chosen in the SQL Server LOV.
        ViewBag.Message = "Form is valid. The selected lookup values are shown below.";
        await LoadLookupsAsync(model, cancellationToken);
        return View(model);
    }

    private async Task LoadLookupsAsync(
        CourseViewModel model,
        CancellationToken cancellationToken)
    {
        model.CourseTypes = _lookupService.GetCourseTypes()
            .Select(x => new SelectListItem(x.Text, x.Value))
            .ToList();

        model.Statuses = _lookupService.GetStatuses()
            .Select(x => new SelectListItem(x.Text, x.Value))
            .ToList();

        model.OracleLovItems = (await _lookupService.GetOracleLovAsync(cancellationToken))
            .Select(x => new SelectListItem(x.Text, x.Value))
            .ToList();

        model.SqlServerJobTypes = (await _lookupService.GetSqlServerJobTypesAsync(cancellationToken))
            .ToList();
    }
}