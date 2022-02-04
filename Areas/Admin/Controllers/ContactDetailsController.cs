using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactDetailsController : Controller
    {

        private readonly Context _context;
        public ContactDetailsController(Context context)
        {
            _context = context;
        }



        // GET: ContactDetailsController
        public ActionResult Index()
        {
            List<ContactDetails> details = _context.contactDetails.ToList();
            return View(details);
        }


        // GET: ContactDetailsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContactDetails details)
        {

            ContactDetails newDetails = new ContactDetails();
            newDetails.Address = details.Address;
            newDetails.Description = details.Description;
            newDetails.Email = details.Email;
            newDetails.OpenClosed = details.OpenClosed;
            newDetails.PhoneHotline = details.PhoneHotline;
            newDetails.PhoneMobile = details.PhoneMobile;
            newDetails.MapUrl = details.MapUrl;

            await _context.contactDetails.AddAsync(newDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: ContactDetailsController/Edit/5
        public async Task<ActionResult> Update(int id)
        {
            ContactDetails contactDetails = await _context.contactDetails.FindAsync(id);
            return View(contactDetails);
        }

        // POST: ContactDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, ContactDetails details)
        {
            ContactDetails contactDetails = await _context.contactDetails.FindAsync(id);

            contactDetails.Address = details.Address;
            contactDetails.Description = details.Description;
            contactDetails.Email = details.Email;
            contactDetails.OpenClosed = details.OpenClosed;
            contactDetails.PhoneHotline = details.PhoneHotline;
            contactDetails.PhoneMobile = details.PhoneMobile;
            contactDetails.MapUrl = details.MapUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ContactDetailsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            ContactDetails contactDetails = await _context.contactDetails.FindAsync(id);
            _context.contactDetails.Remove(contactDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
