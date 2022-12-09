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
    public class PostCatServiceImpl : PostCatService
    {
        private readonly ApplicationDbContext _db;
        public PostCatServiceImpl(ApplicationDbContext db)
        {
            _db= db;
        }
        public bool Create(PostCat postCat)
        {
            try
            {
                _db.PostCats.Add(postCat);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public PostCat FindById(int id)
        {
            return _db.PostCats.Find(id);
        }

        public IQueryable<PostCat> GetAllPostCats()
        {
            return _db.PostCats.Include(pc => pc.ApplicationUser);
        }

        public IQueryable<PostCat> GetAllPostCatsWithSearch(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return GetAllPostCats();
            }
            else
            {
                return _db.PostCats.Where(pc => pc.Name.Contains(keyword))
                    .Include(pc => pc.ApplicationUser);
            }
        }

        public bool Remove(PostCat postCat)
        {
            try
            {
                _db.PostCats.Remove(postCat);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(PostCat postCat)
        {
            try
            {
                _db.PostCats.Update(postCat);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
