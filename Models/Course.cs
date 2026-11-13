using System.ComponentModel.DataAnnotations;

namespace LookupsDemo.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Course name")]
    [StringLength(100)]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Course type")]
    public string CourseType { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Status")]
    public string Status { get; set; } = string.Empty;
}
