using LibraryShop.DTOs;

namespace LibraryShop.Services;

public interface IOrderService
{
    Task<OrderDetailDto> GetOrderByIdAsync(int id);
    Task<OrderDetailDto> CreateOrderAsync(OrderCreateDto dto);
    Task                 FulfillOrderAsync(int id);
}
