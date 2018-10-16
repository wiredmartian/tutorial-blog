using BlogWebApp.Data;
using BlogWebApp.Models;
using BlogWebApp.Services.Interface;
using BlogWebApp.Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost _post = new PostService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AddPost()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPost(PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            return View(model);
        }
    }
}