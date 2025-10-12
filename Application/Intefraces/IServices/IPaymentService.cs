using Application.DTOs._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface IPaymentService
    {
        public Task<ApiResponse> PayCashAsync();
        public Task<ApiResponse> PayVisaAsync();
    }
}
