using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using EcommerceWeb.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    [Area("admin")]
    [Route("admin/post")]
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly PostCatService _postCatService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        public PostController(PostCatService postCatService,
            PostService postService, UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostEnvironment)
        {
            _postCatService = postCatService;
            _postService = postService;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
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
            ViewData["CountTotal"] = _postService.CountTotal();
            ViewData["CountActive"] = _postService.CountByStatus(true);
            ViewData["CountInActive"] = _postService.CountByStatus(false);
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
            IQueryable<Post> posts = _postService.GetAllPostsWithFilter(searchString, sortOrder, status);
            return View(PaginatedList<Post>.Create(posts, pageNumber ?? 1, pageSize));
        }

        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.SelectItemPostCat = _postCatService.GetAllPostCats();
            return View();
        }

        [Route("create")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Post post, IFormFile file)
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
                var path = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images/posts", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);

                //Add more field to Product
                post.ImageUrl = @"\images\posts\" + fileName;

                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                post.ApplicationUser = user;
                post.Created = DateTime.Now;
                if (_postService.Create(post))
                {
                    TempData["success"] = "Create Post Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Create Post Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.SelectItemPostCat = _postCatService.GetAllPostCats();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Post Not Found";
                return RedirectToAction(nameof(Index));
            }
            var post = _postService.FindById(id);
            if (post == null)
            {
                TempData["error"] = "Post Not Found";
                return RedirectToAction(nameof(Index));
            }
            if (post.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, post.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (_postService.Remove(post))
            {
                TempData["success"] = "Delete Post Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Delete Post Fail";
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("edit")]
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                TempData["error"] = "Post Not Found";
                return RedirectToAction(nameof(Index));
            }
            var post = _postService.FindById(id);
            if (post == null)
            {
                TempData["error"] = "Post Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SelectItemPostCat = _postCatService.GetAllPostCats();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public IActionResult Edit(Post post, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    //Remove OldImage
                    if (post.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, post.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //Add NewImage
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = HelpersFile.GenerateFileName(file.FileName);
                    var path = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images/posts", fileName);
                    var fileStream = new FileStream(path, FileMode.Create);
                    file.CopyTo(fileStream);
                    post.ImageUrl = @"\images\posts\" + fileName;
                }
                if (_postService.Update(post))
                {
                    TempData["success"] = "Update Post Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Update Post Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.SelectItemPostCat = _postCatService.GetAllPostCats();
            return View(post);
        }
    }
}
