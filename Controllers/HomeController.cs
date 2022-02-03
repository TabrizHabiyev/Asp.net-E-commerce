using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Migrations;
using Asp.net_E_commerce.Models;
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


        public IActionResult Index()
        {
            List<SliderCompany> slider = _context.SliderCompany.ToList();
            ViewData["SliderCompany"] = slider;
            return View();
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
            List<SliderCompany> slider = _context.SliderCompany.ToList();
            return PartialView("_CompanySliderPartial" , slider);
        }


    }
}
