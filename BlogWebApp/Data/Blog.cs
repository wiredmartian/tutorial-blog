using BlogWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogWebApp.Data
{
    public class Blog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BlogId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }
}