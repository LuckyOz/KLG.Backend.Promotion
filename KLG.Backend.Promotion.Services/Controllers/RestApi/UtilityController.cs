using DotNetCore.CAP;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace KLG.Backend.Promotion.Services.Controllers.RestApi;

[AllowAnonymous]
[Route("[controller]")]
public class UtilityController : KLGApiController<DefaultDbContext>
{
    public UtilityController(
        IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
        IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
    : base(dbProvider, configuration, messageProvider, logger) { }

    [HttpPost]
    [Route("reset")]
    public async Task<IActionResult> Reset([FromServices] IBootstrapper bootstrapper)
    {
        await bootstrapper.DisposeAsync();

        await _dbProvider.ResetTablesAsync(typeof(PromoWorkflow));

        await bootstrapper.BootstrapAsync();

        return Ok();
    }

    [HttpPost]
    [Route("resetall")]
    public async Task<IActionResult> ResetAll([FromServices] IBootstrapper bootstrapper)
    {
        await bootstrapper.DisposeAsync();

        await _dbProvider.ResetTablesAsync();

        await bootstrapper.BootstrapAsync();

        return Ok();
    }
}
