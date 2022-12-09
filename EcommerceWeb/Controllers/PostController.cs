using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EcommerceWeb.Controllers
{
    [Route("post")]
    public class PostController : Controller
    {
        private readonly PostService _postService;
        public PostController(PostService postService)
        {
            _postService = postService;
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
            IQueryable<Post> posts = _postService.GetAllPostsWithSearch(searchString);
            return View(PaginatedList<Post>.Create(posts, pageNumber ?? 1, pageSize));
        }

        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var post = _postService.FindById(id);
            if (post == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }
    }
}
