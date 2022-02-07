using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoryBrand> categoryBrands { get; set; }
    }
}
