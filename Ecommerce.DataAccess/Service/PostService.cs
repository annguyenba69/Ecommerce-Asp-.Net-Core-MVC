using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface PostService
    {
        public IQueryable<Post> GetAllPosts();
        public IQueryable<Post> GetAllPostsWithSearch(string keyword);
        public IQueryable<Post> GetAllPostsWithFilter(string keyword, string sortOrder, string status);
        public bool Create(Post post);
        public bool Update(Post post);
        public bool Remove(Post post);
        public int CountTotal();
        public int CountByStatus(bool status);
        public Post FindById(int id);
    }
}
