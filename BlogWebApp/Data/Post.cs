using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogWebApp.Data
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PostId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public string Blurb { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastUpdated { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        public string Slug { get; set; }
        public string Author { get; set; }
        public bool Cancelled { get; set; }

        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}