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
    [Authorize]
    [RoutePrefix("blog")]
    public class BlogController : Controller
    {
        private readonly IPost _post = new Services.Logic.PostService();
        // GET: Blog
        [HttpGet]
        [Route("add")]
        public ViewResult AddPost()
        {
            return View();
        }
        [HttpPost]
        [Route("add")]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var post = _post.AddPost(model);
            if (post != null)
                return RedirectToAction("GetPostBySlug", new { slug = post.Slug });
            return View(model);
        }
        [HttpGet]
        [Route("update/{id}")]
        public ActionResult UpdatePost(Guid id)
        {
            var post = _post.GetPostById(id);
            if (post != null)
            {
                return View(new PostViewModel
                {
                    Title = post.Title,
                    PostID = post.PostID,
                    HtmlBody = post.Body,
                    Tags = post.Tags,
                    Blurb = post.Blurb
                });
            }
            return Redirect("/blog/posts");
        }
        [HttpPost]
        [Route("update/{id}")]
        public ActionResult UpdatePost(PostViewModel model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var post = _post.PutPost(model);
            if (post != null)
                return RedirectToAction("GetPostBySlug", new { slug = post.Slug });
            return View(model);
        }
        [HttpGet]
        [Route("posts")]
        public ViewResult GetListOfPosts(int? page)
        {
            return View(_post.GetPosts(page));
        }
        [HttpGet]
        [Route("preview/{slug}")]
        public JsonResult PreviewPost(string slug)
        {
            if (Request.IsAjaxRequest())
            {
                var post = _post.GetPost(slug);
                return Json(post, JsonRequestBehavior.AllowGet);
            }
            return Json(new { });
        }
        [HttpGet]
        [Route("post/{slug}")]
        public ViewResult GetPostBySlug(string slug)
        {
            var post = _post.GetPost(slug);
            return View(post);
        }

        [HttpGet]
        [Route("manage")]
        public ViewResult ManageBlog()
        {
            var posts = _post.ManageBlog();
            return View(posts);
        }

        [HttpPost]
        [Route("post/{id}")]
        public JsonResult RemovePost(Guid id)
        {
            string message = string.Empty;
            bool success = false;
            if (id == Guid.Empty || id == null)
            {
                message = "Post Not Found";
            }
            var post = _post.RemovePost(id);
            if (!post)
            {
                message = "Failed to remove post";
            } else
            {
                message = "Successfully removed post";
                success = true;
            }
            return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}