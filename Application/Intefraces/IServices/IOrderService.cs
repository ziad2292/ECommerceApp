using Application.DTOs._Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface IOrderService
    {
        Task<ApiResponse> CreateOrderAsync(PaymentMethodEnum paymentMethod);
        Task<ApiResponse> GetOrderAsync(Guid orderId);
        Task<ApiResponse> GetUserPastOrdersAsync(Guid userId);
        Task<ApiResponse> GetAllOrders();
    }
}
