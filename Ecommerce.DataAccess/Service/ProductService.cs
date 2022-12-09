using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface ProductService
    {
        public IQueryable<Product> GetAllProducts();
        public IQueryable<Product> GetAllProductsWithSearch(string keyword);
        public IQueryable<Product> GetAllProductsWithFilter(string keyword, string sortOrder, string status, int? productCatId = null);
        public IQueryable<Product> GetNewestProducts(int number);
        public IQueryable<Product> GetRandomProducts(int number);
        public Product FindById(int id);
        public Product FindOnlyProduct(int id);
        public bool Create (Product product);
        public bool Update (Product product);
        public bool Remove(Product product);
        public int CountTotal();
        public int CountByStatusPublic(bool status);
        public int CountByStatusWarehouse(bool status);
    }
}
