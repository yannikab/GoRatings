using GoRatings.Api.Contracts.Properties;
using GoRatings.Services.PropertyPersister.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace GoRatings.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IGivenPropertyFactory givenPropertyFactory;

    private readonly IPropertyPersisterService propertyPersisterService;

    public PropertiesController(
        IGivenPropertyFactory givenPropertyFactory,
        IPropertyPersisterService propertyPersisterService)
    {
        this.givenPropertyFactory = givenPropertyFactory;
        this.propertyPersisterService = propertyPersisterService;
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Adds a new property.", Description = "Adds a new property entity, with all its relevant data. A unique id is generated automatically for the property. Upon success, ratings can be added for the property.")]
    [SwaggerResponse(201, "Property added successfully.", Type = typeof(CreatePropertyResponse))]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(500, "An error has occurred.")]
    public async Task<IActionResult> CreateProperty([FromBody][SwaggerRequestBody("Property creation request.", Required = true)] CreatePropertyRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetErrors());

        IGivenProperty givenProperty = request.ToGivenProperty(givenPropertyFactory);

        var storedProperty = await propertyPersisterService.AddAsync(givenProperty);

        var createProperyResponse = storedProperty.ToCreatePropertyResponse();

        return CreatedAtAction(nameof(GetProperty), new { entityUid = createProperyResponse.EntityUid }, createProperyResponse);
    }

    [HttpGet("{entityUid:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Retrieves the property with the specified unique id.", Description = "Searches stored properties for one with the specified unique id. If found, it is returned with all its associated data.")]
    [SwaggerResponse(200, "Property found.", Type = typeof(GetPropertyResponse))]
    [SwaggerResponse(404, "No property found with the given unique id.")]
    [SwaggerResponse(500, "An error has occurred.")]
    public async Task<IActionResult> GetProperty([SwaggerParameter("The unique id of the property to be retrieved.", Required = true)] Guid entityUid)
    {
        var storedProperty = await propertyPersisterService.GetAsync(entityUid);

        return Ok(storedProperty.ToGetPropertyResponse());
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Retrieves all stored properties.", Description = "Retrieves all stored properties, including inactive ones, with all their associated data.")]
    [SwaggerResponse(200, "All available properties retrieved.", Type = typeof(IEnumerable<GetPropertyResponse>))]
    [SwaggerResponse(500, "An error has occurred.")]
    public async Task<IActionResult> GetAllProperties()
    {
        var storedProperties = await propertyPersisterService.GetAllAsync();

        return Ok(storedProperties.Select(sp => sp.ToGetPropertyResponse()));
    }
}
