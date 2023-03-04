using GoRatings.Api.Contracts.RealEstateAgents;
using GoRatings.Services.RealEstateAgentPersister.Exceptions;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RealEstateAgentsController : ControllerBase
{
    private readonly IRealEstateAgentPersisterService persisterService;
    private readonly ILogger<RatingsController> log;
    private readonly IHostApplicationLifetime hostApplicationLifetime;

    public RealEstateAgentsController(
        IRealEstateAgentPersisterService persisterService,
        ILogger<RatingsController> log,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        this.persisterService = persisterService;
        this.log = log;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Adds a new real estate agent.", Description = "Adds a new real estate agent entity, with all its relevant data. A unique id is generated automatically for the real estate agent. Upon success, ratings can be added for the real estate agent.")]
    [SwaggerResponse(201, "Real estate agent added successfully.", Type = typeof(CreateRealEstateAgentResponse))]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(500, "An error has occurred.")]
    public IActionResult CreateRealEstateAgent([FromBody][SwaggerRequestBody("Real estate agent creation request.", Required = true)] CreateRealEstateAgentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrors());

        IGivenRealEstateAgent givenRealEstateAgent = request.ToGivenRealEstateAgent();

        try
        {
            var storedRealEstateAgent = persisterService.Add(givenRealEstateAgent);

            var createRealEstateAgentResponse = storedRealEstateAgent.ToCreateRealEstateAgentResponse();

            return CreatedAtAction(nameof(GetRealEstateAgent), new { entityUid = createRealEstateAgentResponse.EntityUid }, createRealEstateAgentResponse);
        }
        catch (Exception ex) when (!ex.IsCritical())
        {
            log.Error(ex);

            return StatusCode(500);
        }
        catch (Exception ex)
        {
            log.Critical(ex);

            hostApplicationLifetime.StopApplication();

            return StatusCode(500);
        }
    }

    [HttpGet("{entityUid:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Retrieves the real estate agent with the specified unique id.", Description = "Searches stored real estate agents for one with the specified unique id. If found, it is returned with all its associated data.")]
    [SwaggerResponse(200, "Real estate agent found.", Type = typeof(GetRealEstateAgentResponse))]
    [SwaggerResponse(404, "No property found with the given unique id.")]
    [SwaggerResponse(500, "An error has occurred.")]
    public IActionResult GetRealEstateAgent([SwaggerParameter("The unique id of the real estate agent to be retrieved.", Required = true)] Guid entityUid)
    {
        try
        {
            var storedRealEstateAgent = persisterService.Get(entityUid);

            return Ok(storedRealEstateAgent.ToGetRealEstateAgentResponse());
        }
        catch (RealEstateAgentDoesNotExistException ex)
        {
            log.Info(ex);

            return NotFound(ex.Message);
        }
        catch (Exception ex) when (!ex.IsCritical())
        {
            log.Error(ex);

            return StatusCode(500);
        }
        catch (Exception ex)
        {
            log.Critical(ex);

            hostApplicationLifetime.StopApplication();

            return StatusCode(500);
        }
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Retrieves all stored real estate agents.", Description = "Retrieves all stored real estate agents, including inactive ones, with all their associated data.")]
    [SwaggerResponse(200, "All available real estate agents retrieved.", Type = typeof(IEnumerable<GetRealEstateAgentResponse>))]
    [SwaggerResponse(500, "An error has occurred.")]
    public IActionResult GetAllRealEstateAgents()
    {
        try
        {
            var storedRealEstateAgents = persisterService.GetAll();

            return Ok(storedRealEstateAgents.Select(sp => sp.ToGetRealEstateAgentResponse()));
        }
        catch (Exception ex) when (!ex.IsCritical())
        {
            log.Error(ex);

            return StatusCode(500);
        }
        catch (Exception ex)
        {
            log.Critical(ex);

            hostApplicationLifetime.StopApplication();

            return StatusCode(500);
        }
    }
}
