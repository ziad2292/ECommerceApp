using Application.DTOs._Common;
using Application.DTOs.Order;
using Application.Intefraces._Common;
using Application.Intefraces.IServices;
using Application.Intefraces.Repositories;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _paymentService = paymentService;
        }

        public async Task<ApiResponse> CreateOrderAsync(PaymentMethodEnum paymentMethod)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                //Get User Id
                Guid userId;
                Guid.TryParse(_currentUserService.UserId, out userId);


                //Validate Cart
                var cart = await _unitOfWork.ShoppingCarts.GetByUserIdAsync(userId);
                if (cart == null || !cart.Items.Any())
                    return new ApiResponse()
                    {
                        IsSuccess = false,
                        Message = "Cart is empty"
                    };

                //Create Payment
                Payment payment = new Payment()
                {
                    Id = Guid.NewGuid(),
                    Amount = 0,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethodId = (int)paymentMethod,
                    PaymentStatusId = (int)PaymentStatusEnum.Paid,
                    UserID = userId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "AUTO"
                };

                //Create Order
                Order order = new Order()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatusId = (int)OrderStatusEnum.Confirmed,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "AUTO",
                    TotalAmount = 0,
                    PaymentId = payment.Id,

                };

                //Calculate Total
                decimal total = 0;
                foreach (var item in cart.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product == null || product.Stock < item.Quantity)
                        return new ApiResponse()
                        {
                            IsSuccess = false,
                            Message = $"Product {product?.Name} is unavailable"
                        };

                    decimal price = product.Price * item.Quantity;
                    total += price;

                    OrderItem orderItem = new OrderItem()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        Price = price,
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                    };  

                    await _unitOfWork.OrderItems.AddAsync(orderItem);
                }

                order.TotalAmount = total;
                payment.Amount = total;

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.Payments.AddAsync(payment);

                //Process Payment
                ApiResponse? paymentResponse = null;
                switch(paymentMethod){
                    case PaymentMethodEnum.Cash:
                        paymentResponse = await _paymentService.PayCashAsync();
                        break;

                    case PaymentMethodEnum.Visa:
                        paymentResponse = await _paymentService.PayVisaAsync();
                        break;
                }

                if (!paymentResponse!.IsSuccess)
                    return new ApiResponse()
                    {
                        IsSuccess = false,
                        Message = "Payment failed"
                    };

                //Update Stock
                foreach(var item in cart.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    product!.Stock -= item.Quantity;
                    await _unitOfWork.Products.UpdateAsync(product);
                }


                //Committing
                await _unitOfWork.ShoppingCartItems.EmptyShoppingCartAsync(cart.Id);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Order placed successfully",
                };

            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransactionAsync();
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Order failed"
                };
            }
        }

        public async Task<ApiResponse> GetOrderAsync(Guid orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);

            if (order == null)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Order doesn't exist"
                };

            return new ApiResponse<OrderDto>()
            {
                IsSuccess = true,
                Message = "Order returned successfully",
                Data = new OrderDto() { Date = order.OrderDate, Id = order.Id, OrderStatus = (OrderStatusEnum)order.OrderStatusId, PaymentId = order.PaymentId, TotoalAmount = order.TotalAmount, UserId = order.UserId }
            };
        }

        public async Task<ApiResponse> GetUserPastOrdersAsync(Guid userId)
        {
            var orders = await _unitOfWork.Orders.GetUserPastOrdersAsync(userId);

            List<OrderDto> orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto() { Date = order.OrderDate, Id = order.Id, OrderStatus = (OrderStatusEnum)order.OrderStatusId, PaymentId = order.PaymentId, TotoalAmount = order.TotalAmount, UserId = order.UserId });
            }

            return new ApiResponse<List<OrderDto>>()
            {
                IsSuccess = true,
                Message = "All orders returned successfully",
                Data = orderDtos
            };
        }

        public async Task<ApiResponse> GetAllOrders()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();

            List<OrderDto> orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto() { Date = order.OrderDate, Id = order.Id, OrderStatus = (OrderStatusEnum)order.OrderStatusId, PaymentId = order.PaymentId, TotoalAmount = order.TotalAmount, UserId = order.UserId });
            }

            return new ApiResponse<List<OrderDto>>()
            {
                IsSuccess = true,
                Message = "All orders returned successfully",
                Data = orderDtos
            };
        }
    }
}
