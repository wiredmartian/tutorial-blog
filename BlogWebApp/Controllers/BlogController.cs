using BlogWebApp.Models;
using BlogWebApp.Services.Interface;
using BlogWebApp.Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogWebApp.Controllers
{
    [Authorize]
    [RoutePrefix("blog")]
    public class BlogController : Controller
    {
        public BlogController(): this(new PostService()) { }
        private readonly IPost _post;

        public BlogController(IPost _post)
        {
            this._post = _post;
        }
        // GET: Blog
        [HttpGet]
        [Route("add")]
        public ViewResult AddPost()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        [Route("add")]
        [ValidateInput(false)]
        public async Task<ActionResult> AddPost(PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var post = await _post.AddPost(model);
            if (post != null)
                return RedirectToAction("GetPostBySlug", new { slug = post.Slug });
            return View(model);
        }
        [HttpGet]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdatePost(Guid id)
        {
            var post = await _post.GetPostById(id);
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
        [AllowAnonymous]
        [Route("posts")]
        public ViewResult GetListOfPosts(int? page)
        {
            var posts = _post.GetPosts(page);
            return View(posts);
        }
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
        [Route("post/{slug}")]
        public async Task<ViewResult> GetPostBySlug(string slug)
        {
            var post = await _post.GetPost(slug);
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