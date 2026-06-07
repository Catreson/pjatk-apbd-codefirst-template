using LibraryShop.Exceptions;
using LibraryShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET /api/customers/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            return Ok(customer);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
