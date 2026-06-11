using Application.Intefraces.IServices;
using Domain.Entities;
using Domain.Enums;
using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Authorize]
    public class OrderController : CustomControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add-order")]
        public async Task<IActionResult> CreateOrder(PaymentMethodEnum paymentMethod)
        {
            var response = await _orderService.CreateOrderAsync(paymentMethod);

            return response.Message switch
            {
                "Order placed successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpGet("get-order")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var response = await _orderService.GetOrderAsync(orderId);

            return response.Message switch
            {
                "Order returned successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllOrders();

            return response.Message switch
            {
                "All orders returned successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpGet("get-user-orders")]
        public async Task<IActionResult> GetUserPastOrders(Guid userId)
        {
            var response = await _orderService.GetUserPastOrdersAsync(userId);

            return response.Message switch
            {
                "All orders returned successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }
    }
}
