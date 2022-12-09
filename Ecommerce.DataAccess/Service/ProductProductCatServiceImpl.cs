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
    public class ProductProductCatServiceImpl : ProductProductCatService
    {
        private readonly ApplicationDbContext _db;
        public ProductProductCatServiceImpl(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(ProductProductCat productProductCat)
        {
            try
            {                        
                _db.ProductProductCats.Add(productProductCat);
                return _db.SaveChanges() > 0;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Remove(ProductProductCat productProductCat)
        {
            try
            {
                _db.ProductProductCats.Remove(productProductCat);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public List<ProductProductCat> GetAllProductProductCatByProductId(int productId)
        {
            return _db.ProductProductCats.Where(ppc => ppc.ProductId == productId).ToList();
        }

        public ProductProductCat GetLastChildProductCat(int productId)
        {
            return _db.ProductProductCats.Where(ppc => ppc.ProductId == productId)
                .OrderByDescending(ppc => ppc.ProductCatId).First();
        }

        public int CountProductsBelongToProductCatId(int? productCatId)
        {
            if(productCatId != null)
            {
                return _db.ProductProductCats.Where(ppc => ppc.ProductCatId == productCatId).Count();
            }
            else
            {
                return _db.Products.Count();
            }
        }
    }
}
