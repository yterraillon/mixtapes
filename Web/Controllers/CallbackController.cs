using Application.ConnectService.Commands.VerifySpotifyAuthentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("spotify/[controller]")]
public class CallbackController : Controller
{
    private readonly IMediator _mediator;

    public CallbackController(IMediator mediator) => _mediator = mediator;
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string code, [FromQuery] string state)
    {
        if (string.IsNullOrEmpty(code)) return BadRequest();
        
        var response = await _mediator.Send(new VerifySpotifyAuthentication.Request(code, state));
        
        return response.IsSuccess
            ? Redirect("http://localhost:5029/")
            : BadRequest();
    }
}