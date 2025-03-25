using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

public class ClassesResponse
{
    public int ? Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? StudentQuantity { get; set; }
    public int? SubjectQuantity { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; }
    public GradeLevelResponse? GradeLevel { get; set; }

    public AcademicYearResponse? AcademicYear { get; set; }

    public UserResponse? User { get; set; }

    public ClassTypeResponse? ClassType { get; set; }

}

