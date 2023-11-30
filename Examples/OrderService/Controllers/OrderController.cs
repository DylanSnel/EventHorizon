using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Commands;
using OrderService.Context;
using OrderService.Queries;

namespace OrderService.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> Get(int id)
    {
        try
        {
            var order = await _mediator.Send(new GetOrderQuery { Id = id });
            return Ok(order);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] CreateOrderCommand command)
    {
        var order = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Order>> Update(int id, [FromBody] UpdateOrderCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        try
        {
            var order = await _mediator.Send(command);
            return Ok(order);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

}
