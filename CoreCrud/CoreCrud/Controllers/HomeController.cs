using CoreCrud.Models;
using CoreCrud.mydatabase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCrud.Controllers
{
    
    public class HomeController : Controller
    {
        clsss43Context db = new clsss43Context();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("name", "Prabhakar Tiwari");
            HttpContext.Session.GetString("name");
            
            List<EmpModel> modobj = new List<EmpModel>();
            var res = db.Students.ToList();
            foreach (var item in res)
            {
                modobj.Add(new EmpModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Mobile = item.Mobile
                });
            }


            return View(modobj);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddEmp()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddEmp(EmpModel modobj)
        {
            Student tbl = new Student();
            tbl.Id = modobj.Id;
            tbl.Name = modobj.Name;
            tbl.Email = modobj.Email;
            tbl.Mobile = modobj.Mobile;

            if (modobj.Id == 0)
            {
                db.Students.Add(tbl);
                db.SaveChanges();
            }
            else
            {
                db.Entry(tbl).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            EmpModel modobj = new EmpModel();
            var edititem = db.Students.Where(a => a.Id == id).First();
            modobj.Id = edititem.Id;
            modobj.Name = edititem.Name;
            modobj.Email = edititem.Email;
            modobj.Mobile = edititem.Mobile;

            return View("AddEmp",modobj);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            clsss43Context db = new clsss43Context();
            var delitem = db.Students.Where(a => a.Id == id).First();
            db.Students.Remove(delitem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
