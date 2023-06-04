using DotNetCore.CAP;
using KLG.Backend.Promotion.Models.Request;
using KLG.Backend.Promotion.Services.Business.Employees;
using KLG.Backend.Promotion.Services.Configuration;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using KLG.Backend.Promotion.Models.Message;
using Microsoft.AspNetCore.Authorization;

namespace KLG.Backend.Promotion.Services.Controllers.RestApi;

/// <summary>
/// API controller to manage employee records.
/// </summary>
[Route("[controller]")]
[SwaggerTag("API to manage employee records")]
public class EmployeeController : KLGApiController<DefaultDbContext>
{
    private EmployeeManager _employeeManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeController"/> class.
    /// </summary>
    /// <param name="dbProvider">The database provider.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="messageProvider">The messaging provider.</param>
    /// <param name="capPublisher">The CAP publisher.</param>
    /// <param name="logger">The logger.</param>
    public EmployeeController(
        IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
        IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
        : base(dbProvider, configuration, messageProvider, logger)
    {
        _employeeManager = new EmployeeManager(_dbProvider, _messageProvider, _configuration);
    }

    /// <summary>
    /// Retrieves a list of all employees.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> containing the list of employees as JSON data.</returns>
    [HttpGet]
    public async Task<IActionResult> Read()
    {
        return Ok(await _employeeManager.GetAll());
    }

    /// <summary>
    /// Retrieves a single employee record by its ID.
    /// </summary>
    /// <param name="id">The ID of the employee record to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the requested employee record as JSON data,
    /// or a bad request response if the ID is invalid or the employee record is not found.</returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> ReadById(string id)
    {
        return Ok(await _employeeManager.GetByIdAsync(id));
    }

    /// <summary>
    /// Creates a new employee record.
    /// </summary>
    /// <param name="employee">The employee data to create.</param>
    /// <returns>An <see cref="IActionResult"/> containing the newly created employee data as JSON data.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDTO employee)
    {
        try
        {
            return Ok(await _employeeManager.Create(KLGMapper.Map<Employee>(employee)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing employee record.
    /// </summary>
    /// <param name="employee">The updated employee data.</param>
    /// <returns>An <see cref="IActionResult"/> containing the updated employee data as JSON data.</returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeDTO employee)
    {
        try
        {
            return Ok(await _employeeManager.Update(KLGMapper.Map<Employee>(employee)));
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest("Database concurrency error. The record already changed since the last time you retrieved it.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary
    /// Deletes an employee record.
    /// </summary>
    /// <param name="id">The ID of the employee record to delete.</param>
    /// <returns>An <see cref="IActionResult"/> indicating whether the delete operation was successful.</returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _employeeManager.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(string id)
    {
        try
        {
            await _employeeManager.RequestToDelete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("pub_message")]
    public async Task<Employee> Create(Employee employee)
    {
        var minimumRecruitedAge = _configuration.GetConfig<PromotionConfiguration>()?.MinimumAge;

        var message = KLGMapper.Map<EmployeeCreated>(employee);
        await _messageProvider.PublishAsync(message);

        return employee;
    }




}