using Ecommerce.DataAccess.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public class PostServiceImpl : PostService
    {
        private readonly ApplicationDbContext _db;
        public PostServiceImpl(ApplicationDbContext db)
        {
            _db = db;
        }

        public int CountByStatus(bool status)
        {
            return _db.Posts.Where(p => p.Status == status).Count();
        }

        public int CountTotal()
        {
            return _db.Posts.Count();
        }

        public bool Create(Post post)
        {
            try
            {
                _db.Posts.Add(post);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public Post FindById(int id)
        {
            return _db.Posts.Include(p => p.PostCat)
                .Include(p => p.ApplicationUser)
                .FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Post> GetAllPosts()
        {
            return _db.Posts.Include(p => p.ApplicationUser)
                .Include(p => p.PostCat);
        }

        public IQueryable<Post> GetAllPostsWithFilter(string keyword, string sortOrder, string status)
        {
            IQueryable<Post> posts = GetAllPostsWithSearch(keyword);
            switch (status)
            {
                case "active":
                    posts = posts.Where(p => p.Status);
                    break;
                case "inactive":
                    posts = posts.Where(p => !p.Status);
                    break;
                default:
                    posts = posts;
                    break;
            }
            switch (sortOrder)
            {
                case "name_desc":
                    posts = posts.OrderByDescending(p => p.Title);
                    break;
                case "Date":
                    posts = posts.OrderBy(p => p.Created);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(p => p.Created);
                    break;
                default:
                    posts = posts.OrderBy(p => p.Title);
                    break;
            }
            return posts;
        }

        public IQueryable<Post> GetAllPostsWithSearch(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return GetAllPosts();
            }
            else
            {
                return _db.Posts.Where(p => p.Title.Contains(keyword))
                .Include(p => p.ApplicationUser)
                .Include(p => p.PostCat);
            }
        }

        public bool Remove(Post post)
        {
            try
            {
                _db.Posts.Remove(post);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Post post)
        {
            try
            {
                _db.Posts.Update(post);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
