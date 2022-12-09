using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class ProductCat
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Category Name Is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Product Category ParentId Is Required")]
        public int ParentId { get;set; }
        public DateTime Created { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public ICollection<ProductProductCat> ProductProductCats { get; set; }
    }
}
