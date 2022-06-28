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
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<ApplicationUser>> Register(ApplicationUser applicationUser)
        {
            
            if(ModelState.IsValid)
            {
                applicationUser.UserName = applicationUser.Email;
                var result = await userManager.CreateAsync(applicationUser, applicationUser.Password);
                if(result.Succeeded)
                {
                    await signInManager.SignInAsync(applicationUser, isPersistent: false);
                    return new Result<ApplicationUser>
                    {
                        Success = true,
                        ResponseObject= applicationUser,
                    };
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                

            }
            return new Result<ApplicationUser>
            {
                Success = false,
                ResponseMessage =ModelState.Values.ToList()[0].Errors.ToList()[0].ErrorMessage,
            };

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<Result<LoginViewModel>> Login(LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync
                     (loginViewModel.Email, loginViewModel.Password,
                     loginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return new Result<LoginViewModel>
                    {
                        Success = true,
                        ResponseObject = loginViewModel,
                    };
                }
            }
            return new Result<LoginViewModel>
            {
                Success = false,
                ResponseMessage = "Invalid Login",
            };

        }
        [HttpGet("Logout")]
        public async Task<bool> Logout()
        {
            await signInManager.SignOutAsync();
            return true;
        }


    }
}
