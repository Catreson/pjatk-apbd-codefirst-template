using LibraryShop.DTOs;
using LibraryShop.Exceptions;
using LibraryShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET /api/orders/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    // POST /api/orders
    // Body: { "customerId": 1, "items": [ { "bookId": 5, "quantity": 2 } ] }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
    {
        try
        {
            var created = await _orderService.CreateOrderAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message); // 400 Bad Request
        }
    }

    // PUT /api/orders/2/fulfill
    // No body needed - calling this endpoint is the action
    [HttpPut("{id}/fulfill")]
    public async Task<IActionResult> Fulfill(int id)
    {
        try
        {
            await _orderService.FulfillOrderAsync(id);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message); // 409 Conflict
        }
    }
}
