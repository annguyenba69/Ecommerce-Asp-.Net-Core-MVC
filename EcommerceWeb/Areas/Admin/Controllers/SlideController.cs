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
    [Route("admin/slide")]
    public class SlideController : Controller
    {
        private readonly SlideService _slideService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public SlideController (SlideService slideService, IWebHostEnvironment hostEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _slideService = slideService;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(string sortOrder, int? pageNumber,
            string currentFilter, string status)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["Status"] = status;
            ViewData["CountTotal"] = _slideService.CountTotal();
            int pageSize = 6;
            IQueryable<Slide> slides = _slideService.GetAllSlidesWithFilter(sortOrder, status);
            return View(PaginatedList<Slide>.Create(slides, pageNumber ?? 1, pageSize));
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(Slide slide, IFormFile file)
        {
            if (file == null)
            {
                ModelState.AddModelError("ImageUrl", "Image Is Required");
            }
            if (ModelState.IsValid)
            {
                //Upload File
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = HelpersFile.GenerateFileName(file.FileName);
                var path = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images/slides", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);

                //Add To Slide
                slide.ImageUrl = @"\images\slides\" + fileName;
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                slide.ApplicationUser = user;
                slide.Created = DateTime.Now;
                if (_slideService.Create(slide))
                {
                    TempData["success"] = "Create Slide Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Create Slide Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Slide Not Found";
                return RedirectToAction(nameof(Index));
            }
            var slide = _slideService.FindById(id);
            if (slide == null)
            {
                TempData["error"] = "Slide Not Found";
                return RedirectToAction(nameof(Index));
            }
            if (slide.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, slide.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (_slideService.Remove(slide))
            {
                TempData["success"] = "Delete Slide Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Delete Slide Fail";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Route("edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Slide Not Found";
                return RedirectToAction(nameof(Index));
            }
            var slide = _slideService.FindById(id);
            if (slide == null)
            {
                TempData["error"] = "Slide Not Found";
                return RedirectToAction(nameof(Index));
            }
            slide.Status = !slide.Status;
            if (_slideService.Update(slide))
            {
                TempData["success"] = "Update Status Slide Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Update Status Slide Fail";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
