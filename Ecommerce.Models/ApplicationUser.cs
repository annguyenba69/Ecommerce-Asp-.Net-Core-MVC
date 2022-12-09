using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ProductCat> ProductCats { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostCat> PostCats { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Slide> Slides { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
    }
}
