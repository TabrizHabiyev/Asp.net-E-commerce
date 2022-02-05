using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.ViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {
        private readonly Context _context;
        public SubscribeViewComponent(Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            Subscribe subscribe = _context.subscribe.FirstOrDefault();

            return View(await Task.FromResult(subscribe));
        }
    }
}
