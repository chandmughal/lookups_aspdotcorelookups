namespace LookupsDemo.Models;

// One row returned by the SQL Server Job Type LOV.
public class JobTypeLovItem
{
    public string JobType { get; set; } = "";
    public string JobDescription { get; set; } = "";
    public string MachineType { get; set; } = "";
    public string CategoryHead { get; set; } = "";
    public string Concern { get; set; } = "";
    public string TypeStatus { get; set; } = "";
    public string CuttingDepartment { get; set; } = "";
    public string StitchingDepartment { get; set; } = "";
    public string InspectionDepartment { get; set; } = "";
    public string PackingDepartment { get; set; } = "";
}