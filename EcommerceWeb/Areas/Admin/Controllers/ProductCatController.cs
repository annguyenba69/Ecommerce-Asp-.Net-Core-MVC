using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using EcommerceWeb.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    [Area("admin")]
    [Route("admin/productcat")]
    public class ProductCatController : Controller
    {
        private readonly ProductCatService _productCatService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductCatController(ProductCatService productCatService,
            UserManager<ApplicationUser> userManager)
        {
            _productCatService = productCatService;
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
            var productCats = _productCatService.GetAllProductCatsWithSearch(searchString).ToList();
            List<ProductCatDataTree> productCatsDataTree = ProductCatsDataTree(productCats);
            return View(productCatsDataTree);
        }

        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.ProductCatsDataTree = ProductCatsDataTree();
            ProductCat productCat = new ProductCat
            {
                Created = DateTime.Now
            };
            return View(productCat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(ProductCat productCat)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                productCat.ApplicationUser = user;
                if (_productCatService.Create(productCat))
                {
                    TempData["success"] = "Create Product Category Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Create Product Category Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(productCat);
        }

        [Route("edit")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Product Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            ProductCat productCat = _productCatService.FindById(id.Value);
            if (productCat == null)
            {
                TempData["error"] = "Product Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProductCatsDataTree = ProductCatsDataTree();
            return View(productCat);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public IActionResult Edit(ProductCat productCat)
        {
            if (ModelState.IsValid)
            {
                if (_productCatService.Update(productCat))
                {
                    TempData["success"] = "Update Product Category Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Update Product Category Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(productCat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Product Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            ProductCat productCat = _productCatService.FindById(id.Value);
            if (productCat == null)
            {
                TempData["error"] = "Product Category Not Found";
                return RedirectToAction(nameof(Index));
            }
            List<ProductCat> productCats = _productCatService.GetAllProductCats();
            if (!HelpersProductCat.checkChild(productCats, productCat.Id))
            {
                if (_productCatService.Remove(productCat))
                {
                    TempData["success"] = "Delete Product Category Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Delete Product Category Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["error"] = "Please Delete Child Product Category First";
                return RedirectToAction(nameof(Index));
            }
        }

        private List<ProductCatDataTree> ProductCatsDataTree(List<ProductCat>? productCats = null)
        {
            if(productCats == null)
            {
                productCats = _productCatService.GetAllProductCats();
            }
            List<ProductCatDataTree> productCatsDataTree = HelpersProductCat.getDataTree(productCats);
            return productCatsDataTree;
        }
    }
}
