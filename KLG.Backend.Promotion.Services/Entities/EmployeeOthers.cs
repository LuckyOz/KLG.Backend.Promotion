/*
 * These entities are meant to test foreign key dependencies among entities.
 * 
 * 
 * 
 */

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KLG.Library.Microservice.DataAccess;

namespace KLG.Backend.Promotion.Services.Entities;

// Create your entity model by inheriting KLGModelBase and using Data Annotation.
[Table("employeecompetency")]
public class EmployeeCompetency : KLGModelBase
{
    [Column("employeeid"), MaxLength(40)]
    [ForeignKey("Employee")]
    public string EmployeeId { get; set; }
    public Employee Employee { get; set; }

    [Column("expertise"), MaxLength(100)]
    public string Expertise { get; set; }

    [Column("attaineddate")]
    public DateTime AttainedDate { get; set; }

    public ICollection<EmployeeTraining> EmployeeTraining { get; set; }
}

[Table("employetraining")]
public class EmployeeTraining : KLGModelBase
{
    [Column("competencyid"), MaxLength(40)]
    [ForeignKey("Competency")]
    public string CompetencyId { get; set; }
    public EmployeeCompetency Competency { get; set; }

    [Column("trainingname"), MaxLength(100)]
    public string TrainingName { get; set; }
}

[Table("employefamily")]
public class EmployeeFamily : KLGModelBase
{
    [Column("employeeid"), MaxLength(40)]
    [ForeignKey("Employee")]
    public string EmployeeId { get; set; }
    public Employee Employee { get; set; }

    [Column("familyname"), MaxLength(100)]
    public string FamilyName { get; set; }
}

[Table("position")]
public class Position : KLGModelBase
{
    [Column("positionname"), MaxLength(100)]
    public string PositionName { get; set; }
}

[Table("employeeposition")]
public class EmployeePosition : KLGModelBase
{
    [Column("employeeid"), MaxLength(40)]
    [ForeignKey("Employee")]
    public string EmployeeId { get; set; }
    public Employee Employee { get; set; }

    [Column("positionid"), MaxLength(40)]
    [ForeignKey("Position")]
    public string PositionId { get; set; }
    public Position Position { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
