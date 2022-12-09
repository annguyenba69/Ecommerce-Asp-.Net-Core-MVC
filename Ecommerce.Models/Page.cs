using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        [ValidateNever]
        public DateTime Created { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
