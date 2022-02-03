using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using FrontToBack.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeaturesBannerController : Controller
    {
     
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;
        public FeaturesBannerController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        // GET: FeaturesBannerController
        public ActionResult Index()
        {
            List<FeaturesBanner> sliders = _context.FeaturesBanners.ToList();
            return View(sliders);
        }



        // GET: FeaturesBannerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FeaturesBannerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeaturesBannerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FeaturesBanner featuresBanner)
        {

            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                ModelState.AddModelError("Photo", "Do not empty");
            }

            if (!featuresBanner.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "only image");
                return View();
            }
            if (featuresBanner.Photo.IsCorrectSize(300))
            {
                ModelState.AddModelError("Photo", "300den yuxari ola bilmez");
                return View();
            }

            FeaturesBanner newFeacture = new FeaturesBanner();
            newFeacture.Name = featuresBanner.Name;
            string fileName = await featuresBanner.Photo.SaveImageAsync(_env.WebRootPath, "assets/images/");
            newFeacture.Url = fileName;
            newFeacture.Description = featuresBanner.Description;
            await _context.FeaturesBanners.AddAsync(newFeacture);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: FeaturesBannerController/Edit/5
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            FeaturesBanner dbFeature = await _context.FeaturesBanners.FindAsync(id);
            if (dbFeature == null) return NotFound();
            return View(dbFeature);
        }


        // POST: FeaturesBannerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, FeaturesBanner featuresBanner)
        {
            if (id == null) return NotFound();

            if (featuresBanner.Photo != null)
            {
                if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                {
                    ModelState.AddModelError("Photo", "Do not empty");
                }

                if (!featuresBanner.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "only image");
                    return View();
                }
                if (featuresBanner.Photo.IsCorrectSize(300))
                {
                    ModelState.AddModelError("Photo", "300den yuxari ola bilmez");
                    return View();
                }
                FeaturesBanner dbfeatures = await _context.FeaturesBanners.FindAsync(id);

                string path = Path.Combine(_env.WebRootPath, "assets/images/brand/", dbfeatures.Url);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                dbfeatures.Name = featuresBanner.Name;
                string fileName = await featuresBanner.Photo.SaveImageAsync(_env.WebRootPath, "assets/images/brand/");
                dbfeatures.Url = fileName;
                dbfeatures.Description = featuresBanner.Description;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: FeaturesBannerController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
          FeaturesBanner dbFeatures = await _context.FeaturesBanners.FindAsync(id);
            if (dbFeatures == null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "assets/images/", dbFeatures.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.FeaturesBanners.Remove(dbFeatures);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
