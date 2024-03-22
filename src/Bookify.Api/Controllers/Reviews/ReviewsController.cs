using Asp.Versioning;
using Bookify.Application.Reviews.addReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Reviews;

[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/reviews")]
[ApiController]
public class ReviewsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] AddReviewRequest request, CancellationToken cancellationToken)
    {
        var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok();
    }
}