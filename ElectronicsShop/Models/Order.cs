using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public double OrderCost { get; set; }

        [Required]
        public int PieceCount { get; set; }

        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
