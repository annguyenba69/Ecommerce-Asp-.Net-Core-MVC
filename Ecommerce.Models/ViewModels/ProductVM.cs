using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class ProductVM
    {
        [Required]
        public Product Product { get; set; }
        [Required(ErrorMessage ="Product Category Is Required")]
        public int ProductCatId { get;set; }
    }
}
