using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/user")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        
        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index(int? pageNumber, string searchString, string currentFilter)
        {
            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var users = _userManager.Users;

            int pageSize = 6;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Fullname.Contains(searchString) || u.UserName.Contains(searchString));
            }

            users =  users.Include(u => u.UserRoles)
                .ThenInclude(u => u.Role);

            ViewBag.Total = users.Count();
            return View(PaginatedList<ApplicationUser>.Create(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Route("create")]
        public IActionResult Create()
        {
            SelectListItemRoles();
            return View();
        }

        private void SelectListItemRoles()
        {
            IEnumerable<SelectListItem> Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            });
            ViewBag.Roles = Roles;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(ApplicationUserVM applicationUserVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserDTO applicationUserDTO = applicationUserVM.ApplicationUserDTO;
                var userCheck = await _userManager.FindByEmailAsync(applicationUserDTO.Email);
                if(userCheck == null)
                {
                    var applicationUser = new ApplicationUser
                    {
                        Fullname = applicationUserDTO.Fullname,
                        Address = applicationUserDTO.Address,
                        PhoneNumber = applicationUserDTO.PhoneNumber,
                        Dob = applicationUserDTO.Dob,
                        Email = applicationUserDTO.Email,
                        UserName = applicationUserDTO.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };
                    var result = await _userManager.CreateAsync(applicationUser, applicationUserDTO.Password);
                    if (result.Succeeded)
                    {
                        var role = await _roleManager.FindByIdAsync(applicationUserVM.ApplicationUserRole.RoleId);
                        await _userManager.AddToRoleAsync(applicationUser, role.Name);
                        TempData["success"] = "Create User Successfull";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        if(result.Errors.Count() > 0)
                        {
                            foreach(var error in result.Errors)
                            {
                                ModelState.AddModelError("Email", error.Description);
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists");
                };
            }
            SelectListItemRoles();
            return View(applicationUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public async Task<IActionResult> Delete(string? id)
        {
            if(id == null)
            {
                TempData["error"] = "User Not Found";
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                TempData["error"] = "User Not Found";
                return RedirectToAction(nameof(Index));
            }
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "Delete User Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Delete User Failed";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
