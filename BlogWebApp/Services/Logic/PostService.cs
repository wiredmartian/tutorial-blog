using BlogWebApp.Data;
using BlogWebApp.Models;
using BlogWebApp.Services.Handlers;
using BlogWebApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlogWebApp.Services.Logic
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();


        public Post AddPost(PostViewModel model)
        {
            FileHandler handler = new FileHandler(model.Image);
            var FileData = handler.UploadImage();

            Post post = new Post()
            {
                Title = model.Title,
                Body = model.HtmlBody,
                Tags = model.Tags,
                Author = _GetAuthor(),
                PostId = model.PostID
            };
            post.Slug = GenerateSlugFromTitle(post.Title.Trim());
            post.Blurb = model.Blurb;
            
            Blog blog = GetBlog();

            post.BlogId = blog.BlogId;
            post.Date = DateTime.Now;
            post.LastUpdated = DateTime.Now;
            post.PostId = Guid.NewGuid();
            
            if (!string.IsNullOrEmpty(FileData.FileName) && FileData.IsUploaded == true)
            {
                post.ImageUrl = FileData.FileName;
            }
            else
            {
                post.ImageUrl = "/Content/Images/Blog/placeholder.png";
            }
            _context.Posts.Add(post);
            _CommitChanges();
            return post;
        }
        public Blog GetBlog()
        {
            ApplicationUser user = GetUser();

            Blog blog = _context.Blogs.FirstOrDefault(b => b.UserId.Equals(user.Id));
            if (blog != null)
            {
                return blog;
            }
            else
            {
                Blog newblog = new Blog
                {
                    UserId = user.Id
                };
                _context.Blogs.Add(newblog);
                _CommitChanges();
                return newblog;
            }
        }

        public async Task<SinglePostViewModel> GetPost(string slug)
        {
            Post _post = new Post();
            SinglePostViewModel post = new SinglePostViewModel();
            _post = await _context.Posts.Where(x => x.Slug.Equals(slug) && x.Cancelled == false).FirstOrDefaultAsync();

            if (_post != null)
            {
                _post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId.Equals(_post.PostId));
                if (_post != null)
                {
                    post.PostID = _post.PostId;
                    post.Title = _post.Title;
                    post.Slug = _post.Slug;
                    post.Body = _post.Body;
                    post.Blurb = _post.Blurb;
                    post.ImageUrl = _post.ImageUrl;
                    post.Author = _post.Author;
                    post.Date = _post.Date;
                    post.Tags = _post.Tags;
                }
            }
            
            return post;
        }
        public SinglePostViewModel GetPostById(Guid postID)
        {
            SinglePostViewModel post = _context.Posts.Where(x => x.PostId.Equals(postID) && x.Cancelled == false)
                .Select(x => new SinglePostViewModel
                {
                    PostID = x.PostId,
                    Title = x.Title,
                    Slug = x.Slug,
                    Body = x.Body,
                    Blurb = x.Blurb,
                    ImageUrl = x.ImageUrl,
                    Author = x.Author,
                    Date = x.Date,
                    Tags = x.Tags
                }).FirstOrDefault();
            return post;
        }

        public IEnumerable<SinglePostViewModel> GetPosts(int? page)
        {
            IEnumerable<SinglePostViewModel> posts = _context.Posts.Where(x => x.Cancelled == false)
                .Select(x => new SinglePostViewModel
                {
                    PostID = x.PostId,
                    Title = x.Title,
                    Slug = x.Slug,
                    Blurb = x.Blurb,
                    ImageUrl = x.ImageUrl,
                    Author = x.Author
            }).ToList().ToPagedList(page ?? 1, 6); // if null set 1, pageSize 6
            return posts;
        }
        public IEnumerable<ManageBlogViewModel> ManageBlog()
        {
            var user = GetUser();
            var blog = _context.Blogs.FirstOrDefault(b => b.UserId == user.Id);
            if (blog != null)
            {
                var posts = _context.Posts.Where(p => p.BlogId == blog.BlogId && p.Cancelled == false)
                    .Select(x => new ManageBlogViewModel
                    {
                        PostID = x.PostId,
                        Title = x.Title,
                        Slug = x.Slug,
                        Created = x.Date
                    });
                return posts;
            }
            return null;
        }
        public ApplicationUser GetUser()
        {
            ApplicationUser webAppUser = _context.Users.FirstOrDefault(u => u.UserName.Equals(HttpContext.Current.User.Identity.Name));
            return webAppUser;
        }

        public SinglePostViewModel PutPost(PostViewModel post)
        {
            if (_ValidGuid(post.PostID))
            {
                FileHandler handler = new FileHandler(post.Image);
                var FileData = handler.UploadImage();
                var singlePost = _GetPost(post.PostID);

                if (singlePost != null)
                {
                    singlePost.Title = post.Title;
                    singlePost.Tags = post.Tags;
                    singlePost.LastUpdated = DateTime.Now; /* Last updated */
                    singlePost.Slug = GenerateSlugFromTitle(post.Title);
                    singlePost.Blurb = post.Blurb;
                    singlePost.Body = post.HtmlBody;

                    if (!string.IsNullOrEmpty(FileData.FileName) && FileData.IsUploaded == true)
                    {
                        singlePost.ImageUrl = FileData.FileName;
                    }
                    _CommitChanges();
                    /* return enough attrs to redirect */
                    return new SinglePostViewModel { Slug = singlePost.Slug, PostID = singlePost.PostId };
                }
            }
            return new SinglePostViewModel();
        }
        public bool RemovePost(Guid postID)
        {
            Post post = _GetPost(postID);
            if (post == null)
            {
                return false;
            }
            post.Cancelled = true;
            _CommitChanges();
            return true;
        }
        public bool DeletePost(Guid postID)
        {
            Post post = _GetPost(postID);
            bool IsPostRemoved = false, IsFileRemoved = false;
            string FileDir = string.Empty;
            if (post == null)
            {
                return IsPostRemoved;
            }
            /** Get file dir from Post */
            FileDir = _GetFileDir(post.ImageUrl);

            /** Remove Post */
            try
            {
                _context.Posts.Remove(post);
                _CommitChanges();
                IsPostRemoved = true;
                /* Remove File */
                IsFileRemoved = _IsFileRemoved(FileDir);
                if (!IsFileRemoved)
                {
                    /** Force File Removal */
                }
                
            } catch
            {
                IsPostRemoved = false;
            }
            return IsPostRemoved;

        }
        private void _CommitChanges()
        {
            _context.SaveChanges();
        }
        private bool _IsFileRemoved(string path)
        {
            string Dir = _GetFileDir(path);
            bool IsRemoved = false;
            if (Directory.Exists(Dir))
            {
                File.Delete(Dir);
                IsRemoved = true;
            }
            return IsRemoved;
        }
        private string _GetFileDir(string path)
        {
            return (string.IsNullOrEmpty(path)) ? "" : HttpContext.Current.Server.MapPath(path);
        }
        private Post _GetPost(Guid postID)
        {
            return _context.Posts.FirstOrDefault(p => p.PostId == postID);
        }
        private Post _EditPost(Post post)
        {
            Post _dbPost = new Post();
            if (_ValidGuid(post.PostId))
            {
                _dbPost = _GetPost(post.PostId);
                if (_dbPost != null)
                {
                    _dbPost.Body = post.Body;
                    _dbPost.Blurb = post.Blurb;
                    _dbPost.Title = post.Title;
                    _dbPost.Tags = post.Tags;
                    _dbPost.ImageUrl = post.ImageUrl;
                    _context.SaveChanges();
                    return _dbPost;
                }
            }
            return _dbPost;
            
        }
        private bool _ValidGuid(Guid guid)
        {
            if (!(guid == Guid.Empty))
            {
                try
                {
                    Guid.Parse(guid.ToString());
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
        private string GenerateSlugFromTitle(string title)
        {
            Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            title = r.Replace(title, "-");
            return title.Replace(' ', '-').ToLower();
        }
        private string _GetAuthor()
        {
            var email = GetUser().Email;
            return email.Substring(0, email.IndexOf('@')).ToUpper();
        }
    }
}
