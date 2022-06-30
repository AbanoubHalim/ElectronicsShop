using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Role Name is Required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
