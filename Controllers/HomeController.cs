using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Controllers
{
    public class HomeController : Controller
    {

        private readonly Context _context;
      
        public HomeController(Context context)
        {
            _context = context;
        }


        public  IActionResult Index()
        {
            List<SliderCompany> slider = _context.sliderCompany.ToList();
            List<FeaturesBanner> banners = _context.featuresBanners.ToList();

            ViewData["SliderCompany"] = slider;
            ViewData["FeaturesBanner"] = banners;
            List<Category> categories = _context.categories.Where(c => c.IsFatured == true).ToList();

            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        // Partials
        // Copany Slider partial
        public IActionResult CompanySlider()
        {
            List<SliderCompany> slider = _context.sliderCompany.ToList();
            return PartialView("_CompanySliderPartial" , slider);
        }


    }
}
