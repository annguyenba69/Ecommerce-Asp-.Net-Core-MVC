using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    [Area("admin")]
    [Route("admin/postcat")]
    public class PostCatController : Controller
    {
        private readonly PostCatService _postCatService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostCatController(PostCatService postCatService, UserManager<ApplicationUser> userManager)
        {
            _postCatService = postCatService;
            _userManager = userManager;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(int? pageNumber, string searchString, string currentFilter)
        {
            int pageSize = 6;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var postCats = _postCatService.GetAllPostCatsWithSearch(searchString);
            return View(PaginatedList<PostCat>.Create(postCats, pageNumber ?? 1, pageSize));
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(PostCat postCat)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                postCat.ApplicationUser = user;
                postCat.Created = DateTime.Now;
                if (_postCatService.Create(postCat))
                {
                    TempData["success"] = "Create PostCat Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Create PostCat Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(postCat);
        }

        [Route("delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Post Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            var postCat = _postCatService.FindById(id);
            if (postCat == null)
            {
                TempData["error"] = "Post Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            if (_postCatService.Remove(postCat))
            {
                TempData["success"] = "Delete Post Category Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {

                TempData["error"] = "Delete Post Category Fail";
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("edit")]
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Post Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            var postCat = _postCatService.FindById(id);
            if (postCat == null)
            {
                TempData["error"] = "Post Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(postCat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public IActionResult Edit(PostCat postCat)
        {
            if (ModelState.IsValid)
            {
                if (_postCatService.Update(postCat))
                {
                    TempData["success"] = "Update Post Category Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Update Post Category Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(postCat);
        }
    }
}
