using LookupsDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LookupsDemo.ViewModels;

public class CourseViewModel
{
    public Course Course { get; set; } = new();

    // Dropdown options are presentation data, so they belong in the ViewModel.
    public List<SelectListItem> CourseTypes { get; set; } = [];
    public List<SelectListItem> Statuses { get; set; } = [];
}
