using Asp.net_E_commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.ViewModels
{
    public class HomeVM
    {
        //Category 
        public IEnumerable<Category> Categories { get; set; }


        //Slider
        public IEnumerable<HomeProductSlider> Sliders { get; set; }


        // For home page blog slider
        public IEnumerable<Blog> blogSlider { get; set; }
       
    }
}
