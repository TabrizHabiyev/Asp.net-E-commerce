using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Controllers
{
    public class ContactController : Controller
    {
        // GET: ContactController1
        private readonly Context _context;
        public ContactController(Context context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            List<ContactDetails> contactDetails = _context.contactDetails.ToList();
            List<SliderCompany> sliderCompanies = _context.sliderCompany.ToList();
            List<FeaturesBanner> featuresBanners = _context.featuresBanners.ToList();

            ViewData["SliderCompany"] = sliderCompanies;
            ViewData["FeaturesBanner"] = featuresBanners;
            return View(contactDetails);
        }



        // POST: ContactController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
