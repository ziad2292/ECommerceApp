using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class SearchFilterDto
    {
        public int? CategoryId {  get; set; }
        public string? Name { get; set; }

        [Range(0, int.MaxValue)]
        [Precision(18, 2)]
        public decimal? MinPrice { get; set; }
        [Range(0, int.MaxValue)]
        [Precision(18, 2)]

        public decimal? MaxPrice { get; set; }
    }
}
