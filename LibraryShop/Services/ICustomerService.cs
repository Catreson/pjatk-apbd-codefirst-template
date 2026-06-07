using LibraryShop.DTOs;

namespace LibraryShop.Services;

public interface ICustomerService
{
    Task<CustomerDetailDto> GetCustomerByIdAsync(int id);
}
