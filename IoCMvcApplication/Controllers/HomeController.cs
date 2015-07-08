using IoCMvcApplication.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IoCMvcApplication.Controllers
{
    public class HomeController : Controller
    {
        private IService Service = null;

        public HomeController(IService service)
        {
            Service = service;
        }

        public ActionResult Index()
        {
            if (Service != null)
            {
                ViewBag.Message = Service.Print();
            }
            else
            {
                ViewBag.Message = "Failed to inject dependency.";
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
