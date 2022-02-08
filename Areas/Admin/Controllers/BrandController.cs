using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly Context _context;

        public BrandController(Context context)
        {
            _context = context;
        }
        // GET: BrandController
        public ActionResult Index()
        {
            List<Brand> brands = _context.brands.Include(b => b.categoryBrands).ThenInclude(c => c.Category).ToList();
            return View(brands);
        }

        // GET: BrandController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandController/Create
        public ActionResult Create()
        {
            ViewBag.IsMainCategory = _context.categories.Where(c => c.IsMain == true).ToList();
            ViewBag.SubCategory = _context.categories.Where(c => c.IsMain == false).ToList();

            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Brand brand, int[] subcategory)
        {
            bool isExist = _context.brands.Any(c => c.Name.ToLower() == brand.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "This category is already exists");
                return RedirectToAction("Index");

            }
            if (subcategory != null)
            {
                Brand newBrand = new Brand();
                newBrand.Name = brand.Name;
                await _context.brands.AddAsync(newBrand);
                await _context.SaveChangesAsync();

                foreach (var item in subcategory)
                {
                    CategoryBrand categoryBrands = new CategoryBrand();
                    categoryBrands.BrandId = newBrand.Id;
                    categoryBrands.CategoryId = item;
                    await _context.AddAsync(categoryBrands);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // GET: BrandController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            Brand brand = await _context.brands.Include(b => b.categoryBrands).ThenInclude(c => c.Category).FirstOrDefaultAsync(c => c.Id == id);
            List<CategoryBrand> SubCategory = await _context.categoryBrands.Include(c => c.Category).Where(x => x.BrandId == brand.Id).ToListAsync();

            List<Category> AllCategory = await _context.categories.Include(c => c.categoryBrands).ThenInclude(c => c.Brand).Where(c => c.IsMain == false).ToListAsync();
            foreach (var item in SubCategory)
            {
                AllCategory.Remove(item.Category);

            }
            ViewBag.checkCategory = SubCategory;
            ViewBag.noneCheck = AllCategory;
            return View(brand);
        }

        // POST: BrandController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, Brand brand, List<int> subcategory)
        {
            bool isExist = _context.brands.Any(c => c.Name.ToLower() == brand.Name.ToLower().Trim());
            Brand newBrand = await _context.brands.FindAsync(id);

            if (isExist && !(newBrand.Name.ToLower() == brand.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", $"{newBrand} brand already exists");
                return RedirectToAction("Edit");
            }

            if (subcategory.Count() == 0)
            {
                ModelState.AddModelError("Name", "Must choose at least 1 category");
                return RedirectToAction("Edit");
            }

            List<int> checkedCategory = _context.categoryBrands.Where(c => c.BrandId == newBrand.Id).Select(i=>i.CategoryId).ToList();

            List<int> addedCategory = subcategory.Except(checkedCategory).ToList();
            List<int> removedCategory = checkedCategory.Except(subcategory).ToList();

            int addedCategoryLength = addedCategory.Count();
            int removedCategoryLength = removedCategory.Count();
            int FullLength = addedCategoryLength + removedCategoryLength;

            newBrand.Name = brand.Name;

            for (int i = 1; i <= FullLength; i++)
            {
                if (addedCategoryLength >= i)
                {
                    CategoryBrand categoryBrand = new CategoryBrand();
                    categoryBrand.BrandId = newBrand.Id;
                    categoryBrand.CategoryId = addedCategory[i-1];
                    await _context.categoryBrands.AddAsync(categoryBrand);
                    await _context.SaveChangesAsync();
                }

                if (removedCategoryLength >= i)
                {
                    CategoryBrand categoryBrand = await _context.categoryBrands.FirstOrDefaultAsync(c => c.CategoryId == removedCategory[i - 1] && c.BrandId == newBrand.Id);
                    _context.categoryBrands.Remove(categoryBrand);
                    await _context.SaveChangesAsync();
                }
             }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: BrandController/Delete/5
        public async  Task<ActionResult> Delete(int id)
        {
            Brand _brand = await _context.brands.FirstOrDefaultAsync(x => x.Id == id);
            if (_brand == null) return RedirectToAction("Delete");
            return View(_brand);
        }

        // POST: BrandController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Brand brand)
        {
            Brand _brand = await _context.brands.FirstOrDefaultAsync(x=>x.Id==brand.Id);
            if (brand==null) return RedirectToAction("Delete");


            List<CategoryBrand> categoryBrand = await _context.categoryBrands.ToListAsync();
            foreach (var item in categoryBrand)
            {
                CategoryBrand DeleteBrand = await _context.categoryBrands.FirstOrDefaultAsync(c=>c.BrandId == brand.Id);
                if (DeleteBrand !=null)
                {
                    _context.categoryBrands.Remove(DeleteBrand);
                    await _context.SaveChangesAsync();
                }
             
            }
            _context.brands.Remove(brand);
            await _context.SaveChangesAsync();

            return View();
        }
    }
}
