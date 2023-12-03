using EventHorizon;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;

namespace PrivacyService.Controllers;

[ApiController]
[Route("[controller]")]
public class PrivacyController(IPublishEndpoint publishEndpoint) : ControllerBase
{

    [HttpDelete("{id}")]
    [StartFlow("Delete User", Tags: ["Privacy"])]
    [TriggersAttribute<DeleteUserCommand>("When the user requests to delete himself on the privacy portal")]
    public async Task<ActionResult> Delete(int id)
    {
        await publishEndpoint.Publish(new DeleteUserCommand { Id = id });
        return NoContent();
    }
}
