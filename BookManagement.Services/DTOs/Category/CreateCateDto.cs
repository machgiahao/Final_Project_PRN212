using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.DTOs.Category
{
    public class CreateCateDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public int? Status { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
