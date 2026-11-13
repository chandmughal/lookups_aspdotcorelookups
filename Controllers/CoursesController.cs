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
    public IActionResult Create()
    {
        var model = new CourseViewModel();
        LoadLookups(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CourseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Dropdown lists must be populated again when returning the form.
            LoadLookups(model);
            return View(model);
        }

        // This demo intentionally does not save to a database.
        // At this point, pass model.Course to your repository/application service.
        ViewBag.Message = "Form is valid. The selected lookup values are shown below.";
        LoadLookups(model);
        return View(model);
    }

    private void LoadLookups(CourseViewModel model)
    {
        model.CourseTypes = _lookupService.GetCourseTypes()
            .Select(x => new SelectListItem(x.Text, x.Value))
            .ToList();

        model.Statuses = _lookupService.GetStatuses()
            .Select(x => new SelectListItem(x.Text, x.Value))
            .ToList();
    }
}
