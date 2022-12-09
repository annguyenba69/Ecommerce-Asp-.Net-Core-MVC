using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class ProductProductCat
    {
        public int ProductId { get; set; }
        public int ProductCatId { get; set; }
        public Product Product { get;set; }
        public ProductCat ProductCat { get; set; }  
    }
}
