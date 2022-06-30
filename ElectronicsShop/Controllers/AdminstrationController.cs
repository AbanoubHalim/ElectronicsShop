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
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminstrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public AdminstrationController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost("CreateRole")]
        public async Task<Result<IdentityRole>>CreateRole(RoleViewModel roleViewModel)
        {
            string message = "";
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new()
                {
                    Name = roleViewModel.RoleName,
                };
               IdentityResult result= await roleManager.CreateAsync(identityRole);
               if(result.Succeeded)
                {
                    return new Result<IdentityRole>
                    {
                        Success = true,
                        ResponseObject = identityRole,
                    };
                }
               else
                    foreach (var err in result.Errors)
                    {
                        message .Concat(err.Description + "   \n");
                    }
            }
            return new Result<IdentityRole>
            {
                Success = false,
                ResponseMessage = message,
            };
        }
       
        [HttpGet("GetRoles")]
        public Result<List<IdentityRole>> GetRoles()
        {

            List<IdentityRole> roles = roleManager.Roles.ToList();

            return new Result<List<IdentityRole>>
            {
                Success = true,
                ResponseObject = roles,
            };
        }

        //[HttpPut("EditRole/{id}")]
        //public async Task<Result<EditRoleViewModel>> EditRole(string id)
        //{
        //    var role = await roleManager.FindByIdAsync(id);
        //    if(role==null)
        //    {
        //        return new Result<EditRoleViewModel>
        //        {
        //            Success = false,
        //            ResponseMessage = "There is no role for this Id"
        //        };
        //    }
        //    var model = new EditRoleViewModel
        //    {
        //        Id = role.Id,
        //        RoleName = role.Name,
        //    };
        //    foreach (var user in userManager.Users)
        //    {
        //       if( await userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            model.Users.Add(user.Name);
        //        }
        //    }
        //    return new Result<EditRoleViewModel>
        //    {
        //        Success = true,
        //        ResponseObject = model
        //    };
        //}

         [HttpGet("GetUsersInRole/{roleId}")]
         public async Task<Result<List<UserRoleViewModel>>>GetUsersInRole(string roleId)
         {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return new Result<List<UserRoleViewModel>>
                {
                    Success = false,
                    ResponseMessage = "There is no role for this Id"
                };
            }
            var model = new List<UserRoleViewModel>();
            foreach(var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if(await userManager.IsInRoleAsync(user,role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return new Result<List<UserRoleViewModel>>
            {
                Success = true,
                ResponseObject = model
            };

        }

        [HttpPost("EditUsersInRole/{roleId}")]
        public async Task<Result<List<UserRoleViewModel>>> EditUsersInRole(string roleId,List<UserRoleViewModel>model)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return new Result<List<UserRoleViewModel>>
                {
                    Success = false,
                    ResponseMessage = "There is no role for this Id"
                };
            }

            for(int i= 0;i<model.Count;i++)
            {
                var user=await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else  
                    continue;
                if(result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return new Result<List<UserRoleViewModel>>
                        {
                            Success = true,
                            ResponseObject = model
                        };
                }
            }
            return new Result<List<UserRoleViewModel>>
            {
                Success = true,
                ResponseObject = model
            };

        }


    }
}
