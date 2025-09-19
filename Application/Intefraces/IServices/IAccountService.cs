using Application.DTOs._Common;
using Application.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface IAccountService
    {
        public ApiResponse GetAllUsers();
        public Task<ApiResponse> GetById(Guid id);
        public Task<ApiResponse> UpdateUser(UserDto user);
        public Task<ApiResponse> DeleteUser(Guid id);
        public Task<ApiResponse> UpdatePassword(Guid id, string oldPassword, string newPassword);
    }
}
