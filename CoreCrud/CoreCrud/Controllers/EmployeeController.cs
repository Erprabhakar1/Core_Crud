using CoreCrud.Models;
using CoreCrud.mydatabase;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreCrud.Controllers
{
    public class EmployeeController : Controller
    {
        clsss43Context obj = new clsss43Context();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(StudentMod objUser)
        {

            var res = obj.Studentregs.Where(a => a.Email == objUser.Email).FirstOrDefault();

            if (res == null)
            {

                TempData["Invalid"] = "Email is not found";
            }

            else
            {
                if (res.Email == objUser.Email && res.Password == objUser.Password)
                {

                    var claims = new[] { new Claim(ClaimTypes.Name, res.Name),
                                        new Claim(ClaimTypes.Email, res.Email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);


                    HttpContext.Session.SetString("Name", objUser.Email);

                    return RedirectToAction("Index", "Home");

                }

                else
                {

                    TempData["Wrong"] = "Wrong Password";

                    return View("Login");
                }


            }


            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login","Employee");
        }

        [HttpGet]
        public IActionResult UserReg()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserReg(StudentMod sobj)
        {
            Studentreg tbl = new Studentreg();
            tbl.Id = sobj.Id;
            tbl.Name = sobj.Name;
            tbl.Email = sobj.Email;
            tbl.Password = sobj.Password;

            obj.Studentregs.Add(tbl);
            obj.SaveChanges();
            return RedirectToAction("Login","Employee");
        }
    }
}
