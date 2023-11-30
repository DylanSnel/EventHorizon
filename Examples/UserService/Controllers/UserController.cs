using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Commands;
using UserService.Context;
using UserService.Queries;

namespace UserService.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        try
        {
            var user = await _mediator.Send(new GetUserQuery { Id = id });
            return Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] CreateUserCommand command)
    {
        var user = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Update(int id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        try
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

}
