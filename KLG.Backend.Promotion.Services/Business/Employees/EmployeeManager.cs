using KLG.Backend.Promotion.Models.Message;
using KLG.Backend.Promotion.Services.Configuration;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using Microsoft.EntityFrameworkCore;

namespace KLG.Backend.Promotion.Services.Business.Employees;

/// <summary>
/// Manages operations related to employees.
/// </summary>
public class EmployeeManager
{
    private IKLGDbProvider<DefaultDbContext> _dbProvider;
    private IKLGMessagingProvider _messageProvider;
    private IKLGConfiguration _configuration;

    public EmployeeManager(IKLGDbProvider<DefaultDbContext> dbProvider,
        IKLGMessagingProvider messageProvider, IKLGConfiguration configuration)
    {
        // By passing the database and message provider, you can mock them when necessary.
        _dbProvider = dbProvider;
        _messageProvider = messageProvider;
        _configuration = configuration;
    }

    /// <summary>
    /// Retrieves all employees.
    /// </summary>
    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _dbProvider.DbContext.Employees.ToListAsync();
    }

    /// <summary>
    /// Retrieves an employee by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>The employee with the specified ID, or null if not found.</returns>
    public async Task<Employee?> GetByIdAsync(string id)
    {
        return await _dbProvider.DbContext.Employees.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="employee">The employee to create.</param>
    /// <returns>The newly created employee.</returns>
    public async Task<Employee> Create(Employee employee)
    {
        ValidateEmployeeEntry(employee);

        // Add entry to the DB context.
        employee.Id = Guid.NewGuid().ToString();
        _dbProvider.DbContext.Employees.Add(employee);

        // Save all changes. SaveChangesAsync will always trigger a DbTransaction
        // to ensure database and messaging processes are always consistent.
        // See the outbox pattern concept.
        await _dbProvider.DbContext.SaveChangesAsync();

        // Send message to the message stream.
        var message = KLGMapper.Map<EmployeeCreated>(employee);
        await _messageProvider.PublishAsync(message);

        // Return the newly created employee.
        // The ID property will be filled with the new employee ID.
        return employee;
    }

    /// <summary>
    /// Updates an existing employee.
    /// </summary>
    /// <param name="employee">The updated employee information.</param>
    /// <returns>The updated employee.</returns>
    public async Task<Employee> Update(Employee employee)
    {
        ValidateEmployeeEntry(employee);

        var updatedEmployee = await GetByIdAsync(employee.Id);

        if (updatedEmployee == null)
            throw new InvalidOperationException($"Employee id:{employee.Id} is not found");

        updatedEmployee.Name = employee.Name;
        updatedEmployee.Age = employee.Age;
        updatedEmployee.LastUpdatedDate = employee.LastUpdatedDate;

        // Attach the entry to the DB context.
        // Make sure to put the correct LastUpdatedDate since it acts as a ConcurrencyCheck property.
        //_dbProvider.DbContext.Attach(employee);

        try
        {
            // SaveChangesAsync will automatically update LastUpdatedDate to the current time.
            await _dbProvider.DbContext.SaveChangesAsync();

            return employee;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException(
                "The record has already changed since the last time you retrieved it.");
        }
    }

   
    /// <summary>
    /// Validates the employee entry for correctness.
    /// </summary>
    /// <param name="employee">The employee to validate.</param>
    private void ValidateEmployeeEntry(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));

        if (string.IsNullOrEmpty(employee.Name))
            throw new ArgumentNullException(nameof(employee.Name), "Name is required");

        var minimumRecruitedAge = _configuration.GetConfig<PromotionConfiguration>()?.MinimumAge;

        if (employee.Age < minimumRecruitedAge)
            throw new ArgumentOutOfRangeException(nameof(employee.Age),
                $"We're not recruiting anyone under {minimumRecruitedAge}");

        if (employee.Age > 150)
            throw new ArgumentOutOfRangeException(nameof(employee.Age), "Invalid age");
    }

    /// <summary>
    /// Deletes an employee by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    public async Task Delete(string id)
    {
        await _dbProvider.DeleteByIdAsync<Employee>(id);
    }

    /// <summary>
    /// Flag an employee as inactive
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    public async Task Deactivate(string id)
    {
        var employee = await GetByIdAsync(id);
        if (employee != null)
        {
            employee.Name += "~deleted";
            employee.ActiveFlag = false;
            await _dbProvider.DbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Requests to delete an employee by their ID.
    /// </summary>
    /// <param name="id">The ID of the employee to request deletion.</param>
    public async Task RequestToDelete(string id)
    {
        var employee = await GetByIdAsync(id);

        if (employee == null)
            throw new InvalidOperationException($"Employee id:{id} is not found");

        // Send message to the message stream.
        var message = new EmployeeDeleteRequest() { Id = id, Name = employee.Name };
        await _messageProvider.PublishAsync(message, nameof(EmployeeDeleteApproval));
    }

    /// <summary>
    /// Checks if an employee deletion is approved based on their name.
    /// </summary>
    /// <param name="name">The name of the employee.</param>
    /// <returns>True if the deletion is approved, false otherwise.</returns>
    public bool DeleteApproval(string name)
    {
        return (!name.StartsWith("~"));
    }
}