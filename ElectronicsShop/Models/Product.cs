using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{

    public class Product
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int PieceCount { get; set; }

        public string Description { get; set; }

        [Required]
        public int Price { get; set; }
        
        [Required]
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
