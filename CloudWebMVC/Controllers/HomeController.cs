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
            if (user.Username == null)
                return RedirectToAction("../Home/Index");
            ViewBag.Message = "Your application description page.";

            return View(user);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SignUp(Models.User user)
        {
            ViewBag.Message = "Login page";
            return View(user);
        }

        [HttpPost]
        public ActionResult SignUp(string username, string password)
        {
            return View("Index");
        }
    }
}