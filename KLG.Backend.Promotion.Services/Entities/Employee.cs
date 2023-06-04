#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using KLG.Library.Microservice.DataAccess;

namespace KLG.Backend.Promotion.Services.Entities;

// Create your entity model by inheriting KLGModelBase and using Data Annotation.
public class Employee : KLGModelBase
{
    [MaxLength(100)]
    public string Name { get; set; }

    public int Age { get; set; }

    public ICollection<EmployeeCompetency> competency { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

