using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        [ValidateNever]
        public string Note { get; set; }
        public DateTime Created { get; set; }
        [ValidateNever]
        public bool PaymentStatus { get;set; }
        [ValidateNever]
        public int PaymentMethodId { get; set; }
        [ValidateNever]
        public PaymentMethod PaymentMethod { get; set; }
        [ValidateNever]
        public int OrderStatusId { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        [ValidateNever]
        public OrderStatus OrderStatus { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
