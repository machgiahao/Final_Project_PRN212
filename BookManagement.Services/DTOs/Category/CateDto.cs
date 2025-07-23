using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.DTOs.Category
{
    public class CateDto
    {
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
    }
}
