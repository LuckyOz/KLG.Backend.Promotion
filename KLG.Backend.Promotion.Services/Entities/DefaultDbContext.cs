
using KLG.Library.Microservice.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace KLG.Backend.Promotion.Services.Entities;

public class DefaultDbContext : KLGDbContext
{
    public DefaultDbContext() { }

    public DefaultDbContext(DbContextOptions opt) : base(opt) { }

    // add your [ENTITY_MODELS] here..
    // public DbSet<[ENTITY_MODELS]> [ENTITY_MODELS] { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Company> Company { get; set; }

    public DbSet<EmployeeCompetency> EmployeesCompetencies { get; set; }
    public DbSet<EmployeeFamily> EmployeeFamily { get; set; }
    public DbSet<EmployeePosition> EmployeePosition { get; set; }
    public DbSet<EmployeeTraining> EmployeeTraining { get; set; }
    public DbSet<Position> Position { get; set; }
    
    // Uncomment the following code when you need to use Fluent API.
    // The fluent API is considered a more advanced feature
    // and it's recommended to use Data Annotations unless your requirements
    // require you to use the fluent API.
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{

    //    // example:
    //    // Define employee table
    //    modelBuilder.Entity<Employee>(entity =>
    //    {
    //        entity.ToTable("employee"); // Set table name
    //        entity.HasKey(e => e.Id); // Set primary key
    //        entity.Property(e => e.Id).HasMaxLength(40); // Set id field length
    //        entity.Property(e => e.Name).HasMaxLength(100); // Set name field length
    //    });
    //}

    // Uncomment the following code when you need to display SQL queries
    // that will be executed by the SaveChanges method. 
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.LogTo(Console.WriteLine);

    //    base.OnConfiguring(optionsBuilder);
    //}
}
