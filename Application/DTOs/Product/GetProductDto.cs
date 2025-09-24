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
        public required string Name { get; set; }

        public required string Description { get; set; }

        [Range(0, int.MaxValue)]
        [Precision(18, 2)]
        public required decimal Price { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public required int CategoryId { get; set; }

        [Range(0, int.MaxValue)]
        public required int Stock { get; set; }
    }
}
