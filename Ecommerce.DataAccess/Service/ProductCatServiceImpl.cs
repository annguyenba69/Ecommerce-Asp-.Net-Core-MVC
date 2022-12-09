using Ecommerce.DataAccess.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public class ProductCatServiceImpl : ProductCatService
    {
        private readonly ApplicationDbContext _db;
        public ProductCatServiceImpl(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(ProductCat productCat)
        {
            try
            {
                _db.ProductCats.Add(productCat);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public ProductCat FindById(int id)
        {
            return _db.ProductCats.Find(id);
        }

        public List<ProductCat> GetAllProductCats()
        {
            return _db.ProductCats.Include(pc => pc.ApplicationUser).ToList();
        }

        public IQueryable<ProductCat> GetAllProductCatsWithSearch(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return _db.ProductCats.Include(pc => pc.ApplicationUser);
            }
            else
            {
                return _db.ProductCats.Where(pc => pc.Name.Contains(keyword))
                    .Include(pc => pc.ApplicationUser);
            }
        }

        public string GetNameProductCat(int? id)
        {
            if(id != null)
            {
                return _db.ProductCats.Find(id).Name;
            }
            else
            {
                return "All Products";
            }
        }

        public bool Remove(ProductCat productCat)
        {
            try
            {
                _db.ProductCats.Remove(productCat);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(ProductCat productCat)
        {
            try
            {
                _db.ProductCats.Update(productCat);
                return _db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                /*                Debug.Write(e.ToString());*/
                return false;
            }
        }
    }
}
