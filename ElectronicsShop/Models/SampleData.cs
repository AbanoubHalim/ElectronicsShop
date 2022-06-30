using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Models
{
    public interface ISampleData
    {
        Task CreateRole();

    }
    public class SampleData:ISampleData
    {
        private ShopContext _context;

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public SampleData(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, ShopContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _context = context;
        }

        public async Task CreateRole()
        {
            try
            {
            string message = "";
            var roleStore = new RoleStore<IdentityRole>(_context);
            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin"
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    Console.WriteLine("correct");
                }
                else
                    foreach (var err in result.Errors)
                    {
                        message.Concat(err.Description + "   \n");
                    }
            }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        public async Task SeedAdminUser()
        {
            var user = new ApplicationUser
            {
                Name="Abanoub Halim",
                PhoneNumber="01280268998",
                UserName = "a.halimovic9999@gmail.com",
                Email = "a.halimovic9999@gmail.com",
                BirthDate="15-9-1996",
                Address="Cairo",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var roleStore = new RoleStore<IdentityRole>(_context);
            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "Admin" });
            }
            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "P@ssw0rd");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(_context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
            }
            await _context.SaveChangesAsync();
        }

       
    }
}
