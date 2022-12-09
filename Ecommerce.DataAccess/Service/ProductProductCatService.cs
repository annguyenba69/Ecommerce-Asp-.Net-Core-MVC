using Ecommerce.DataAccess.Data;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DataAccess.Service
{
    public interface ProductProductCatService 
    {
        public bool Create(ProductProductCat productProductCat);
        public ProductProductCat GetLastChildProductCat(int productId);
        public List<ProductProductCat> GetAllProductProductCatByProductId(int productId);
        public bool Remove(ProductProductCat productProductCat);
        public int CountProductsBelongToProductCatId(int? productCatId);
    }
}
