#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KLG.Library.Microservice.DataAccess;

namespace KLG.Backend.Promotion.Services.Entities;

// Create your entity model by inheriting KLGModelBase and using Data Annotation.
[Table("company")]
public class Company : KLGModelBase
{
    [Column("name"), MaxLength(100)]
    public string Name { get; set; }

    [Column("totalemployee")]
    public int TotalEmployee { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

