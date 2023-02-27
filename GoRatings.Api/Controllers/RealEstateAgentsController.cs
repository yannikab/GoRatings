using GoRatings.Api.Contracts.RealEstateAgents;
using GoRatings.Services.Exceptions.RealEstateAgent;
using GoRatings.Services.Interfaces.RealEstateAgent;
using GoRatings.Services.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class RealEstateAgentsController : ControllerBase
{
    private readonly IRealEstateAgentPersisterService persisterService;

    public RealEstateAgentsController(IRealEstateAgentPersisterService persisterService)
    {
        this.persisterService = persisterService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateRealEstateAgent([FromBody] CreateRealEstateAgentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrors());

        IGivenRealEstateAgent givenRealEstateAgent = request.ToGivenRealEstateAgent();

        try
        {
            var storedRealEstateAgent = persisterService.Add(givenRealEstateAgent);

            return CreatedAtAction(nameof(GetRealEstateAgent), new { entityUid = storedRealEstateAgent.EntityUid }, storedRealEstateAgent);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return StatusCode(500);
        }
    }

    [HttpGet("{entityUid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetRealEstateAgent(Guid entityUid)
    {
        try
        {
            var storedRealEstateAgent = persisterService.Get(entityUid);

            return Ok(storedRealEstateAgent.ToRealEstateAgentResponse());
        }
        catch (RealEstateAgentDoesNotExistException ex)
        {
            Console.WriteLine(ex.Message);

            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return StatusCode(500);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetAllRealEstateAgents()
    {
        try
        {
            var storedRealEstateAgents = persisterService.GetAll();

            return Ok(storedRealEstateAgents.Select(sp => sp.ToRealEstateAgentResponse()));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return StatusCode(500);
        }
    }
}
