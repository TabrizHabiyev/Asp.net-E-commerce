using Asp.net_E_commerce.DAL;
using Asp.net_E_commerce.Models;
using Asp.net_E_commerce.Services;
using Asp.net_E_commerce.ViewModels;
using Asp.net_E_commerce.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Asp.net_E_commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Context _context;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, IWebHostEnvironment env,
            IConfiguration config, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;
            _env = env;
        }


        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }


        public async Task<IActionResult> Index() 
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Index","Home");
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(appUser);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Account");

            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Email = register.Email,
                Gender = register.Gender,
                Newsletter = register.Newsletter,
                Avatar = (register.Gender == "Woman") ? "assets/images/Avatar/woman.png" : "assets/images/Avatar/man.png"
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, register.Password);

            if (!identityResult.Succeeded)
            {

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            CustomerMailList isExists = _context.customerMailLists.FirstOrDefault(x => x.Mail == register.Email);

            if (isExists == null)
            {
                if (register.Newsletter == true)
                {
                    CustomerMailList customerMailList = new CustomerMailList();
                    customerMailList.Mail = register.Email;
                    customerMailList.IsSubscriber = true;
                    await _context.AddAsync(customerMailList);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    CustomerMailList customerMailList = new CustomerMailList();
                    customerMailList.Mail = register.Email;
                    customerMailList.IsSubscriber = false;
                    await _context.AddAsync(customerMailList);
                    await _context.SaveChangesAsync();
                }
            }

            await _userManager.AddToRoleAsync(user, "Member");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var ConfirimLink = Url.Action(nameof(EmailConfirm), "Account", new { email = user.Email, token }, Request.Scheme);


            using (MailMessage mail = new MailMessage())
            {
                string mailFrom = _config["SMTP_CONNECTION_STRING:SmtpMail"];
                string mailTo = register.Email;
                string smtpClient = _config["SMTP_CONNECTION_STRING:SmtpClient"];
                string smtpMailPassword = _config["SMTP_CONNECTION_STRING:SmtpMailPassword"];
                int smtpPort = Convert.ToInt32(_config["SMTP_CONNECTION_STRING:SmtpPort"]);

                mail.From = new MailAddress(mailFrom);
                mail.To.Add(mailTo);
                mail.Subject = "Email Verification";
                mail.Body = $"<a href=\"{ConfirimLink}\">Got to reset password</a>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(smtpClient, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(mailFrom, smtpMailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                TempData["RegisterSuccess"] = "Registration Successful ! Verification sent to your e-mail address";
                return View();
            }
        }


        //  Confirm Email
        public async Task<IActionResult> EmailConfirm(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound();

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            TempData["successConfirimEmail"] = "Email verified successfully, You can login";
            return RedirectToAction("Login", "Account");
        }



        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }


        
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Account");

            if (!ModelState.IsValid) return View();

            AppUser dbUser = await _userManager.FindByNameAsync(login.UserName);


            if (dbUser == null)
            {
                ModelState.AddModelError("", "UserName or Password invalid");
                return View();
            }

            bool emailConfirim = dbUser.EmailConfirmed;
            var roles = await _userManager.GetRolesAsync(dbUser);

            if (roles[0] != "Admin" && !emailConfirim)
            {
                ModelState.AddModelError("", "Verify your email address to login");
                return View();
            }

            if (!dbUser.IsActive)
            {
                ModelState.AddModelError("", "user is deactive");
                return View();
            }


            var singInResult = await _signInManager.PasswordSignInAsync(dbUser, login.Password, true, true);


            if (singInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "is lockout");
                return View();
            }

            if (!singInResult.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password invalid");
                return View();
            }

            
            if (roles[0] == "Admin")
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            };

            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> DeleteFoto()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }

            AppUser dbUser = await _userManager.FindByNameAsync(User.Identity.Name);

            string path = Path.Combine(_env.WebRootPath, dbUser.Avatar);
            string userAvatar = dbUser.Avatar;

            if (userAvatar != "assets/images/Avatar/woman.png" && userAvatar != "assets/images/Avatar/man.png")
            {
                System.IO.File.Delete(path);
                dbUser.Avatar = (dbUser.Gender == "Woman") ? "assets/images/Avatar/woman.png" : "assets/images/Avatar/man.png";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Account");
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(string FirstName,string LastName,string Email,
        string Gender,string Password,string NewPassword,string ConfirmPassword, IFormFile photo) 
        {

            AppUser dbUser = await _userManager.FindByNameAsync(User.Identity.Name);
            string fileName = dbUser.Avatar;

            if (photo != null)
            {
                if (!photo.IsImage())
                {
                    ViewData["ResponsError"] = "Only image";
                    return RedirectToAction("Index", "Account");
                }
                if (photo.IsCorrectSize(3000))
                {
                    ViewData["ResponsError"] = "Photo can be max 15mb";
                    return RedirectToAction("Index", "Account");
                }
                fileName = "assets/images/Avatar/" + await photo.SaveImageAsync(_env.WebRootPath, "assets/images/Avatar/");
            }


            AppUser isExist = await _userManager.FindByEmailAsync(Email);
            if (isExist != null)
            {
                if (isExist.UserName != User.Identity.Name)
                {
                    ViewData["ResponsError"] = "This Email address is available";
                    return RedirectToAction("Index", "Account");
                }

            }

            bool changePassword = false;
            if (NewPassword != null)
            {
                if (NewPassword == ConfirmPassword)
                {
                    var singInResult = await _signInManager.PasswordSignInAsync(dbUser, Password, true, true);

                    if (!singInResult.Succeeded)
                    {
                        ViewData["ResponsError"] = "Old password is not correct";
                        return RedirectToAction("Index", "Account");
                    }
                }
                else
                {
                    ViewData["ResponsError"] = "New passwords do not match";
                    return RedirectToAction("Index", "Account");
                }
                changePassword = true;
            }

            dbUser.FullName = FirstName + " " + LastName;
            dbUser.Email = Email;
            dbUser.Gender = Gender;

            dbUser.Avatar = fileName;

            await _userManager.RemovePasswordAsync(dbUser);

            if (changePassword == true)
            {
                await _userManager.AddPasswordAsync(dbUser, NewPassword);
            }

            return RedirectToAction("Index", "Account");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        // Create role
        public async Task CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            }
        }

    }
}