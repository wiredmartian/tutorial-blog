using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogWebApp.Models
{
    public class PostViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be blank.")]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Post cannot be blank.")]
        [AllowHtml]
        [DataType(DataType.Html)]
        public string HtmlBody { get; set; }

        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Post summary cannot be blank.")]
        [StringLength(maximumLength: 255)]
        [DataType(DataType.MultilineText)]
        public string Blurb { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        public string Tags { get; set; }
        [HiddenInput]
        public Guid PostID { get; set; }
    }
    public class SinglePostViewModel
    {
        public Guid PostID { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public string Tags { get; set; }
        public string ImageUrl { get; set; }
        public string Slug { get; set; }
    }
    public class PostsViewModel
    {
        public List<PostViewModel> Posts { get; set; }
    }
    public class ManageBlogViewModel
    {
        public Guid PostID { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
    }
}