using GoRatings.Api.Contracts.RealEstateAgents;
using GoRatings.Services.RealEstateAgentPersister.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RealEstateAgentsController : ControllerBase
{
    private readonly IGivenRealEstateAgentFactory realEstateAgentFactory;

    private readonly IRealEstateAgentPersisterService realEstateAgentPersisterService;

    public RealEstateAgentsController(
        IGivenRealEstateAgentFactory realEstateAgentFactory,
        IRealEstateAgentPersisterService realEstateAgentPersisterService)
    {
        this.realEstateAgentFactory = realEstateAgentFactory;
        this.realEstateAgentPersisterService = realEstateAgentPersisterService;
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
    public async Task<IActionResult> CreateRealEstateAgent([FromBody][SwaggerRequestBody("Real estate agent creation request.", Required = true)] CreateRealEstateAgentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrors());

        IGivenRealEstateAgent givenRealEstateAgent = request.ToGivenRealEstateAgent(realEstateAgentFactory);

        var storedRealEstateAgent = await realEstateAgentPersisterService.AddAsync(givenRealEstateAgent);

        var createRealEstateAgentResponse = storedRealEstateAgent.ToCreateRealEstateAgentResponse();

        return CreatedAtAction(nameof(GetRealEstateAgent), new { entityUid = createRealEstateAgentResponse.EntityUid }, createRealEstateAgentResponse);
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
    public async Task<IActionResult> GetRealEstateAgent([SwaggerParameter("The unique id of the real estate agent to be retrieved.", Required = true)] Guid entityUid)
    {
        var storedRealEstateAgent = await realEstateAgentPersisterService.GetAsync(entityUid);

        return Ok(storedRealEstateAgent.ToGetRealEstateAgentResponse());
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Retrieves all stored real estate agents.", Description = "Retrieves all stored real estate agents, including inactive ones, with all their associated data.")]
    [SwaggerResponse(200, "All available real estate agents retrieved.", Type = typeof(IEnumerable<GetRealEstateAgentResponse>))]
    [SwaggerResponse(500, "An error has occurred.")]
    public async Task<IActionResult> GetAllRealEstateAgents()
    {
        var storedRealEstateAgents = await realEstateAgentPersisterService.GetAllAsync();

        return Ok(storedRealEstateAgents.Select(sp => sp.ToGetRealEstateAgentResponse()));
    }
}
