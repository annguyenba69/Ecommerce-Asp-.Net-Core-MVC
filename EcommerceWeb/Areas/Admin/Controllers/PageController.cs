using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using EcommerceWeb.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    [Area("admin")]
    [Route("admin/page")]
    public class PageController : Controller
    {
        private readonly PageService _pageService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PageController(PageService pageService, UserManager<ApplicationUser> userManager)
        {
            _pageService = pageService;
            _userManager = userManager;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(string sortOrder, int? pageNumber, string searchString,
            string currentFilter, string status)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["Status"] = status;
            ViewData["CountTotal"] = _pageService.CountTotal();
            ViewData["CountActive"] = _pageService.CountByStatus(true);
            ViewData["CountInActive"] = _pageService.CountByStatus(false);
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
            IQueryable<Page> pages = _pageService.GetAllPagesWithFilter(searchString, sortOrder, status);
            return View(PaginatedList<Page>.Create(pages, pageNumber ?? 1, pageSize));
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                page.ApplicationUser = user;
                page.Created = DateTime.Now;
                if (_pageService.Create(page))
                {
                    TempData["success"] = "Create Page Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Create Page Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Page Not Found";
                return RedirectToAction(nameof(Index));
            }
            var page = _pageService.FindById(id);
            if (page == null)
            {
                TempData["error"] = "Page Not Found";
                return RedirectToAction(nameof(Index));
            }
            if (_pageService.Remove(page))
            {
                TempData["success"] = "Delete Page Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Delete Page Fail";
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("edit")]
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Page Not Found";
                return RedirectToAction(nameof(Index));
            }
            var page = _pageService.FindById(id);
            if (page == null)
            {
                TempData["error"] = "Page Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public IActionResult Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                if (_pageService.Update(page))
                {
                    TempData["success"] = "Update Page Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Update Page Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(page);
        }
    }
}
