using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.ViewModels
{
    public class ApplicationUserVM
    {
        public ApplicationUserDTO ApplicationUserDTO { get; set; }

        [ValidateNever]
        public ApplicationUserRole ApplicationUserRole { get; set; }
    }
}
