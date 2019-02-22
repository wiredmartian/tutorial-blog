using BlogWebApp.Data;
using BlogWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlogWebApp.Services.Interface
{
    public interface IPost
    {
        Post AddPost(PostViewModel model);
        SinglePostViewModel PutPost(PostViewModel post);
        Task<SinglePostViewModel> GetPost(string slug);
        SinglePostViewModel GetPostById(Guid postID);
        IEnumerable<SinglePostViewModel> GetPosts(int? page);
        IEnumerable<ManageBlogViewModel> ManageBlog();
        bool RemovePost(Guid postID);
        bool DeletePost(Guid postID);
    }
}
