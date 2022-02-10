using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Controllers
{
    public class CommentController : Controller
    {

        private readonly Context _context;
        public CommentController(Context context)
        {
            _context = context;
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Comment comment)
        {

            if (comment.Text == null || comment.Text.Length < 25)
                return RedirectToAction("detail", "blog", new { id = comment.BlogId });

            string userId = String.Empty;

            if (User.Identity.IsAuthenticated)
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            else
                return RedirectToAction("Login", "Account");

            bool isExist = _context.comments.Any(c => c.BlogId == comment.BlogId && c.UserId == userId);
            if (isExist)
            {
                return RedirectToAction("detail", "blog", new { id = comment.BlogId });
            }

            try
            {
                Comment _comment = new Comment
                {
                    Text = comment.Text,
                    UserId = userId,
                    BlogId = comment.BlogId,
                    Date = DateTime.Now
                };

                await _context.comments.AddAsync(_comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("detail", "blog", new { id = comment.BlogId });
            }
            catch
            {
                return View();
            }
        }




        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromBody] Comment comment)
        {
            if (comment.Text == null || comment.Text.Length < 25)
                return Ok("Comment field should not be empty or less than 25 characters!");

            string userId = String.Empty;
            if (User.Identity.IsAuthenticated)
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            else
                return RedirectToAction("Login", "Account");


            Comment _comment = _context.comments.FirstOrDefault(c => c.Id == comment.Id);
            if (comment == null) return RedirectToAction("Index", "Home");

            try
            {
                if (_comment.UserId == userId)
                {
                    _comment.Text = comment.Text;
                    await _context.SaveChangesAsync();
                    return Ok("ok");
                };

                return RedirectToAction("detail", "blog", new { id = comment.BlogId });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }



        // Get: CommentController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            string userId = String.Empty;

            if (User.Identity.IsAuthenticated)
                userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Comment comment = await _context.comments.FindAsync(id);
            if (comment == null) return RedirectToAction("Index", "Home");

            try
            {
                if (comment.UserId == userId)
                {
                    _context.comments.Remove(comment);
                    await _context.SaveChangesAsync();
                };

                return RedirectToAction("detail", "blog", new { id = comment.BlogId });
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
