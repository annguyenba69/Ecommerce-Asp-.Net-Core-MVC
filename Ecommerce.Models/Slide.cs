using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Slide
    {
        public int Id { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public bool Status { get; set; }
        [ValidateNever]
        public DateTime Created { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
