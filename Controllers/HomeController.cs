using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Adventure_MVC.Models;

namespace Adventure_MVC.Controllers
{
    public class HomeController : Controller
    {
        private dbAventurasEntities1 dbEntities = new dbAventurasEntities1();

        public ActionResult Index()
        {
            // Mostrará las ultimas 3 Fotos Ingresadas
            var lastFotos = dbEntities.tbFotos
                            .OrderByDescending(f => f.id)
                            .Take(3)
                            .ToList();
            return View(lastFotos.ToList());
        }
    }
}