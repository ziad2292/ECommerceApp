using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class GetProductDto
    {
        public required string? Name { get; set; }

        [Range(0, int.MaxValue)]
        [Precision(18, 2)]
        public required decimal Price { get; set; }

        [Url]
        public required string? ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public required int Stock { get; set; }
    }
}
