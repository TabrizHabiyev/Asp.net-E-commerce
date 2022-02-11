using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly Context _context;


        public ProductController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product> product = _context.products
                .Include(p => p.productPhotos)
                .Include(p => p.Brand)
                .Include(p => p.Campaign)
                .Include(p => p.ColorProducts)
                .Include(p => p.ProductTags)
                .ToList();
            return View(product);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Product product = await _context.products
                .Include(p => p.productPhotos)
                .Include(p => p.Brand)
                .Include(p => p.Campaign)
                .Include(p => p.ColorProducts)
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);
            ViewBag.tags = _context.productTags.Include(p => p.Tag).Where(p => p.ProductId == id).ToList();
            return View(product);
        }

    }
}
