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
    public class ProductServiceImpl : ProductService
    {
        private readonly ApplicationDbContext _db;
        public ProductServiceImpl(ApplicationDbContext db)
        {
            _db = db;
        }

        public int CountByStatusPublic(bool status)
        {
            return _db.Products.Where(p => p.StatusPublic== status).Count();    
        }

        public int CountByStatusWarehouse(bool status)
        {
            return _db.Products.Where(p => p.StatusWarehouse== status).Count();
        }

        public int CountTotal()
        {
            return _db.Products.Count();
        }

        public bool Create(Product product)
        {
            try
            {
                _db.Products.Add(product);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public Product FindById(int id)
        {
            return _db.Products
                .Include(p => p.ProductProductCats)
                    .ThenInclude(ppc => ppc.ProductCat)
                .Include(p => p.OrderProducts)
                .FirstOrDefault(p => p.Id == id);
        }

        public Product FindOnlyProduct(int id)
        {
            return _db.Products.FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Product> GetAllProducts()
        {
            return _db.Products
                .Include(p => p.ApplicationUser)
                .Include(p => p.ProductProductCats)
                    .ThenInclude(ppc => ppc.ProductCat)
                .AsNoTracking();
        }

        public IQueryable<Product> GetAllProductsWithFilter(string keyword, string sortOrder, string status,
            int? productCatId = null)
        {
            IQueryable<Product> products = GetAllProductsWithSearch(keyword);
            if(productCatId != null)
            {
                products  = products.Where(p => p.ProductProductCats.Any(ppc => ppc.ProductCatId == productCatId));
            }
            switch (status)
            {
                case "stock":
                    products = products.Where(p => p.StatusWarehouse);
                    break;
                case "outstock":
                    products = products.Where(p => !p.StatusWarehouse);
                    break;
                default:
                    products = products;
                    break;
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "Date":
                    products = products.OrderBy(p => p.Created);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(p => p.Created);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }
            return products;
        }

        public IQueryable<Product> GetAllProductsWithSearch(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return GetAllProducts();
            }
            else
            {
                return _db.Products
                    .Where(p => p.Name.Contains(keyword))
                    .Include(p => p.ApplicationUser)
                    .Include(p => p.ProductProductCats)
                        .ThenInclude(ppc => ppc.ProductCat)
                    .AsNoTracking();
            }
        }

        public IQueryable<Product> GetNewestProducts(int number)
        {
            return _db.Products.Where(p =>p.StatusPublic).OrderByDescending(p => p.Created).Take(number);
        }

        public IQueryable<Product> GetRandomProducts(int number)
        {
            return _db.Products.OrderBy(p => Guid.NewGuid()).Take(number);
        }

        public bool Remove(Product product)
        {
            try
            {
                _db.Products.Remove(product);
                return _db.SaveChanges() > 0;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Update(Product product)
        {
            try
            {
                _db.Products.Update(product);
                return _db.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
