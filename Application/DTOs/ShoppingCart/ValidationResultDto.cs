using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ShoppingCartDTOs
{
    public class ValidationResultDto
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public Domain.Entities.ShoppingCart? ShoppingCart { get; set; }
        public Domain.Entities.Product? Product { get; set; }
    }
}
