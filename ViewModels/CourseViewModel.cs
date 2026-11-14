using LookupsDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LookupsDemo.ViewModels;

public class CourseViewModel
{
    public Course Course { get; set; } = new();

    public List<SelectListItem> CourseTypes { get; set; } = [];
    public List<SelectListItem> Statuses { get; set; } = [];
    public List<SelectListItem> OracleLovItems { get; set; } = [];
}
