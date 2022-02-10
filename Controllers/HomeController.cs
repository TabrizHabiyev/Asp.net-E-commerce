using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Asp.net_E_commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            //Get new arrive
            List<Product> products = _context.products.Include(p => p.Campaign).Include(p => p.productPhotos).Include(p => p.Brand).ToList();


            ViewData["SliderCompany"] = slider;
            ViewData["FeaturesBanner"] = banners;


            List<Blog> blogs =  _context.Blogs.Include(x => x.BlogPhotos).OrderByDescending(x => x.Id).Take(10).ToList();

            List<Category> categories = _context.categories.Where(c => c.IsFatured == true).ToList();
            List<HomeProductSlider> homeProductSliders = _context.homeProductSliders.Include(x => x.Product).ToList();

            HomeVM homeVm = new HomeVM();
            homeVm.Categories = categories;
            homeVm.Sliders = homeProductSliders;

            homeVm.blogSlider = blogs;

            ViewBag.newarrive = products.OrderByDescending(p => p.Id).Take(8).ToList();
            ViewBag.Featured =  products.Where(x => x.Featured == true).OrderByDescending(x => x.Id).Take(8).ToList();

            return View(homeVm);
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
