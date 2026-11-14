using LookupsDemo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LookupsDemo.ViewModels;

public class CourseViewModel
{
    public Course Course { get; set; } = new();

    public List<SelectListItem> CourseTypes { get; set; } = [];
    public List<SelectListItem> Statuses { get; set; } = [];
    public List<SelectListItem> OracleLovItems { get; set; } = [];

    // Selected value from SHOP.NL_00_01.
    public string? SelectedOracleLovCode { get; set; }

    // SQL Server LOV: all columns are displayed, but only JobType is submitted.
    public List<JobTypeLovItem> SqlServerJobTypes { get; set; } = [];
    public string? SelectedJobType { get; set; }
}