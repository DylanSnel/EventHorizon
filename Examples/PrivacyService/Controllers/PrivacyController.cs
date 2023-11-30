using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;

namespace PrivacyService.Controllers;

[ApiController]
[Route("[controller]")]
public class PrivacyController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await publishEndpoint.Publish(new DeleteUserCommand { Id = id });
        return NoContent();
    }
}
