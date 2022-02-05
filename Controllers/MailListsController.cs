using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Controllers
{
    public class MailListsController : Controller
    {

        // GET: ContactController1
        private readonly Context _context;
        public MailListsController(Context context)
        {
            _context = context;
        }

        // POST: MailListsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Subscribe(string email)
        {
            if (email == null)   return RedirectToAction("Index", "Home");
            if (!IsValid(email)) return RedirectToAction("Index", "Home");

            CustomerMailList isExists =  _context.customerMailLists.FirstOrDefault(x => x.Mail == email);

            if (isExists != null) return RedirectToAction("Index", "Home");

            CustomerMailList customerMailList = new CustomerMailList();
                customerMailList.Mail = email;
                customerMailList.IsSubscriber = true;
                await _context.AddAsync(customerMailList);
                await _context.SaveChangesAsync();

            return RedirectToAction("Index","Home"); 
        }
        private bool IsValid(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }
    }
}
