using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudWebMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard(Models.User user)
        {
            ViewBag.Message = "Your application description page.";

            return View(user);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            ViewBag.Message = "Login page";
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string username, string password)
        {
            return View("Index");
        }
    }
}