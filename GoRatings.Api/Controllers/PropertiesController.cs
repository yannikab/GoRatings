using GoRatings.Api.Contracts.Properties;
using GoRatings.Services.Exceptions.Property;
using GoRatings.Services.Interfaces.Property;
using GoRatings.Services.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyPersisterService persisterService;

    public PropertiesController(IPropertyPersisterService persisterService)
    {
        this.persisterService = persisterService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateProperty([FromBody] CreatePropertyRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrors());

        IGivenProperty givenProperty = request.ToGivenProperty();

        try
        {
            var storedProperty = persisterService.Add(givenProperty);

            return CreatedAtAction(nameof(GetProperty), new { entityUid = storedProperty.EntityUid }, storedProperty);
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
    public IActionResult GetProperty(Guid entityUid)
    {
        try
        {
            var storedProperty = persisterService.Get(entityUid);

            return Ok(storedProperty.ToPropertyResponse());
        }
        catch (PropertyDoesNotExistException ex)
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
    public IActionResult GetAllProperties()
    {
        try
        {
            var storedProperties = persisterService.GetAll();

            return Ok(storedProperties.Select(sp => sp.ToPropertyResponse()));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return StatusCode(500);
        }
    }
}
