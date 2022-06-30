using ElectronicsShop.DTOs;
using ElectronicsShop.Models;
using ElectronicsShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost("Register")]
        public async Task<Result<ApplicationUser>> Register(ApplicationUser applicationUser)
        {
            string errorMessage = "";
            if(ModelState.IsValid)
            {
                applicationUser.UserName = applicationUser.Email;
                var result = await userManager.CreateAsync(applicationUser, applicationUser.PasswordHash);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(applicationUser, isPersistent: false);
                    return new Result<ApplicationUser>
                    {
                        Success = true,
                        ResponseObject = applicationUser,
                    };
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        errorMessage.Concat(err.Description + "   \n");
                    }
                    return new Result<ApplicationUser>
                    {
                        Success = false,
                        ResponseMessage = errorMessage,
                    };
                }
            }
            return new Result<ApplicationUser>
            {
                Success = false,
                ResponseMessage =ModelState.Values.ToList()[0].Errors.ToList()[0].ErrorMessage,
            };

        }

        [HttpPost("Login")]
        public async Task<Result<UserLogedIn>> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync
                     (loginViewModel.Email, loginViewModel.Password,
                     loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    ApplicationUser applicationUser =await userManager.FindByNameAsync(loginViewModel.Email);
                    UserLogedIn user = new()
                    {
                        Name = applicationUser.Name,
                        PhoneNumber = applicationUser.PhoneNumber,
                        Address = applicationUser.Address,
                        Email = applicationUser.Email,
                        BirthDate = applicationUser.BirthDate
                    };
                    if (await userManager.IsInRoleAsync(applicationUser, "Admin"))
                    {
                        user.Role = "Admin";
                    }
                    else if (await userManager.IsInRoleAsync(applicationUser, "User"))
                    {
                        user.Role = "User";
                    }
                    return new Result<UserLogedIn>
                    {
                        Success = true,
                        ResponseObject = user,
                    };
                }
            }
            return new Result<UserLogedIn>
            {
                Success = false,
                ResponseMessage = "Invalid Login",
            };

        }
       
        [Authorize]
        [HttpGet("Logout")]
        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }


    }
}
