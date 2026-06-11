using Application.DTOs._Common;
using Application.Intefraces.IServices;

namespace Application.Services
{
    public class VirtualPaymentService : IPaymentService
    {
        public Task<ApiResponse> PayCashAsync()
        {
            return Task.FromResult(new ApiResponse
            {
                IsSuccess = true,
                Message = "Cash payment approved"
            });
        }

        public Task<ApiResponse> PayVisaAsync()
        {
            return Task.FromResult(new ApiResponse
            {
                IsSuccess = true,
                Message = "Visa payment approved"
            });
        }
    }
}
