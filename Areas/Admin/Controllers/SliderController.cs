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

    public class SliderController : Controller
    {

        private readonly Context _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        // GET: SliderController
        public ActionResult Index()
        {
            List<SliderCompany> sliders = _context.sliderCompany.ToList();
            return View(sliders);
        }


        // GET: SliderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SliderCompany slider)
        {
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
               ModelState.AddModelError("Photo", "Do not empty");
             }

            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "only image");
                return View();
            }
            if (slider.Photo.IsCorrectSize(300))
            {
                ModelState.AddModelError("Photo", "300den yuxari ola bilmez");
                return View();
            }

            SliderCompany newSlider = new SliderCompany();
            newSlider.Name = slider.Name;
            string fileName = await slider.Photo.SaveImageAsync(_env.WebRootPath, "assets/images/brand/");
            newSlider.Url = fileName;

            await _context.sliderCompany.AddAsync(newSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
          }



        // GET: SliderController/Edit/5
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            SliderCompany dbSlider = await _context.sliderCompany.FindAsync(id);
            if (dbSlider == null) return NotFound();
            return View(dbSlider);
        }



        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, SliderCompany slider)
        {
            if (id == null) return NotFound();

            if (slider.Photo != null)
            {
                if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                {
                    ModelState.AddModelError("Photo", "Do not empty");
                }

                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "only image");
                    return View();
                }
                if (slider.Photo.IsCorrectSize(300))
                {
                    ModelState.AddModelError("Photo", "300den yuxari ola bilmez");
                    return View();
                }
                SliderCompany dbSlider = await _context.sliderCompany.FindAsync(id);
                string path = Path.Combine(_env.WebRootPath, "assets/images/brand/", dbSlider.Url);
               
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                dbSlider.Name = slider.Name;
                string fileName = await slider.Photo.SaveImageAsync(_env.WebRootPath, "assets/images/brand/");
                dbSlider.Url = fileName;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: SliderController/Delete/5
        public async Task<IActionResult>  Delete(int? id)
        {
            if (id == null) return NotFound();
            SliderCompany dbSlider = await _context.sliderCompany.FindAsync(id);
            if (dbSlider == null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "assets/images/brand/", dbSlider.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.sliderCompany.Remove(dbSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
