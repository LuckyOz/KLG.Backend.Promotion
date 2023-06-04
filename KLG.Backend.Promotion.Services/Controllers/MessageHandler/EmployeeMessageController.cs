using DotNetCore.CAP;
using KLG.Backend.Promotion.Models.Message;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Backend.Promotion.Services.Business;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice;
using Microsoft.AspNetCore.Mvc;
using KLG.Backend.Promotion.Services.Business.Employees;
using KLG.Backend.Promotion.Services.Business.Companies;

namespace KLG.Backend.Promotion.Services.Controllers.MessageHandler;

[Route("[controller]")]
public class EmployeeMessageController : KLGApiController<DefaultDbContext>
{
    private EmployeeManager _employeeManager;

    public EmployeeMessageController(
        IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
        IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
        : base(dbProvider, configuration, messageProvider, logger)
    {
        _employeeManager = new EmployeeManager(_dbProvider, _messageProvider, _configuration);
    }

    /// <summary>
    /// Subscriber method to handle EmployeeCreated messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee created DTO payload.</param>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeCreated))]
    public async Task EmployeeCreatedSubscriber(KLGMessage p)
    {
        var emp = _messageProvider.GetPayloadAsync<EmployeeCreated>(p)
            ?? throw new InvalidOperationException("Payload is null");

        await new HeadcountManager(_dbProvider).RecruitNewEmployee();
    }

    /// <summary>
    /// Subscriber method to handle "employee.deleted" messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee deleted DTO payload.</param>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleted))]
    public async Task EmployeeDeletedSubscriber(KLGMessage p)
    {
        var emp = _messageProvider.GetPayloadAsync<EmployeeDeleted>(p)
            ?? throw new InvalidOperationException("Payload is null");

        await new HeadcountManager(_dbProvider).EmployeeResignation();
    }

    /// <summary>
    /// Subscriber method to handle EmployeeDeleteRequest messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee delete request DTO payload.</param>
    /// <returns>The employee delete approval DTO.</returns>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleteRequest))]
    public EmployeeDeleteApproval EmployeeDeleteRequestSubscriber(KLGMessage p)
    {
        var message = _messageProvider.GetPayloadAsync<EmployeeDeleteRequest>(p)
            ?? throw new InvalidOperationException("Invalid message");

        return new EmployeeDeleteApproval()
        {
            Id = message.Id,
            Approved = _employeeManager.DeleteApproval(message.Name)
        };
    }

    /// <summary>
    /// Subscriber method to handle EmployeeDeleteApproval messages published by the message provider.
    /// </summary>
    /// <param name="p">The KLG message containing the employee delete approval DTO payload.</param>
    [NonAction]
    [KLGMessageSubscribe(nameof(EmployeeDeleteApproval))]
    public async Task EmployeeDeleteApprovalSubscriber(KLGMessage p)
    {
        var message = _messageProvider.GetPayloadAsync<EmployeeDeleteApproval>(p)
            ?? throw new InvalidOperationException("Invalid message");

        if (message.Approved)
        {
            await _employeeManager.Deactivate(message.Id);
        }
        else
        {
            _logger.Information($"Deletion of employee {message.Id} was rejected");
        }
    }
}
