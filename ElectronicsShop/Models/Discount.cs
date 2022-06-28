using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class Discount
    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public double OnePieceDiscountPercentage { get; set; }

        public double MoreOnePieceDiscountPercentage { get; set; }
    }
}
