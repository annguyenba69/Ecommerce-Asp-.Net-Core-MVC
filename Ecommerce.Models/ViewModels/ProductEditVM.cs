using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class ProductEditVM
    {
        public Product Product { get; set; }
        public int OldProductCatId { get; set; }
        public int NewProductCatId { get; set; }
    }
}
