using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ColorProduct> ColorProducts { get; set; }

    }
}
