using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Controllers
{
    public class BlogController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(Context context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var blog =  _context.Blogs.ToList();

            var photos = _context.BlogPhotos.ToList();

            ViewBag.photos = photos;

            return View(blog);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            List<Comment> comments = await _context.comments.Where(c => c.BlogId == id).Include(x => x.User).ToListAsync();

            var blog = await _context.Blogs.Include(x => x.BlogPhotos).Include(x=>x.User).FirstOrDefaultAsync(x => x.Id == id);


            var tags = await _context.productTags.Where(p => p.ProductId == blog.ProductId).Select(t => t.Tag).ToListAsync();



            // Relatet Post
             List<Blog> relatedPost = await _context.Blogs.Include(x=>x.BlogPhotos)
            .Where(x => x.ProductId == blog.ProductId)
            .OrderByDescending(x => x.Id)
            .Take(3)
            .ToListAsync();

            // RESENT POST
            List<Blog> resentPost = await _context.Blogs.Include(x=>x.BlogPhotos).OrderByDescending(x=>x.Id).Take(4).ToListAsync();

            ViewBag.resentPost = resentPost;
            ViewBag.relatedPost = relatedPost;
            ViewBag.tags = tags;
            ViewBag.comment = comments;
            return View(blog);
        }

        public async Task<IActionResult> Search(string search)
        {
            IEnumerable<Blog> blogs = await _context.Blogs
                .Include(c => c.BlogPhotos)
                .Where(p => p.Title.ToLower().Contains(search.ToLower()))
                .Take(7)
                .ToListAsync();

            return PartialView("_SearchBlogPartial", blogs);
        }
    }
}
