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
        Task<Post> AddPost(PostViewModel model);
        Task<SinglePostViewModel> PutPost(PostViewModel post);
        Task<SinglePostViewModel> GetPost(string slug);
        Task<SinglePostViewModel> GetPostById(Guid postID);
        IEnumerable<SinglePostViewModel> GetPosts(int? page);
        Task<IEnumerable<ManageBlogViewModel>> ManageBlog();
        Task<bool> RemovePost(Guid postID);
        Task<bool> DeletePost(Guid postID);
    }
}
