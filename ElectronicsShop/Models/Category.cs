using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class Category
    {
        [Key]
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
