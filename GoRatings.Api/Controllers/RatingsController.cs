using GoRatings.Contracts;

using Microsoft.AspNetCore.Mvc;

namespace GoRatings.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateRating(CreateRatingRequest request)
    {
        return Ok(request);
    }

    [HttpGet("{entityUid:guid}")]
    public IActionResult GetRating(Guid entityUid)
    {
        return Ok(entityUid);
    }
}
