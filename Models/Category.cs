using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public bool IsFatured { get; set; }
        public string ImageUrl { get; set; }
        public List<Category> SubCategory { get; set; }
        public Category MainCategory { get; set; }
        public bool IsDeleted { get; set; }
        public List<CategoryBrand> categoryBrands { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }

}
