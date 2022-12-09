using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface ProductCatService
    {
        public List<ProductCat> GetAllProductCats();
        public IQueryable<ProductCat> GetAllProductCatsWithSearch(string keyword);
        public bool Create(ProductCat productCat);
        public ProductCat FindById(int id);
        public bool Update (ProductCat productCat);
        public bool Remove(ProductCat productCat);
        public string GetNameProductCat(int? id);
    }
}
