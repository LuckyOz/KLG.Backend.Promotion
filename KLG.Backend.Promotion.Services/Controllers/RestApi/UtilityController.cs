using DotNetCore.CAP;
using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice.Configuration;
using KLG.Library.Microservice.DataAccess;
using KLG.Library.Microservice.Messaging;
using KLG.Library.Microservice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace KLG.Backend.Promotion.Services.Controllers.RestApi;

[Route("[controller]")]
public class UtilityController : KLGApiController<DefaultDbContext>
{
    public UtilityController(
        IKLGDbProvider<DefaultDbContext> dbProvider, IKLGConfiguration configuration,
        IKLGMessagingProvider messageProvider, Serilog.ILogger logger)
    : base(dbProvider, configuration, messageProvider, logger) { }

    /// <summary>
    /// Resets the employee data to its default state.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating whether the reset operation was successful.</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("reset")]
    public async Task<IActionResult> Reset([FromServices] IBootstrapper bootstrapper)
    {
        await bootstrapper.DisposeAsync();

        await _dbProvider.ResetTablesAsync(typeof(Employee));

        await bootstrapper.BootstrapAsync();

        return Ok();
    }

    /// <summary>
    /// Resets all data in the database to its default state.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> indicating whether the reset operation was successful.</returns>
    [AllowAnonymous]
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
