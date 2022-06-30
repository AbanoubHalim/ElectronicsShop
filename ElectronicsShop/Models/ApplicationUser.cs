using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public class ApplicationUser:IdentityUser
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public override string Email { get => base.Email; set => base.Email = value; }

        [Required]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }


        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public string BirthDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
