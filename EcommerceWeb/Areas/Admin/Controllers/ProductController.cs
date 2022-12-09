using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using EcommerceWeb.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    [Area("admin")]
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ProductCatService _productCatService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProductProductCatService _productProductCatService;
        public ProductController(ProductService productService,
            ProductCatService productCatService, IWebHostEnvironment hostEnvironment,
            UserManager<ApplicationUser> userManager, ProductProductCatService productProductCatService)
        {
            _productService = productService;
            _productCatService = productCatService;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _productProductCatService = productProductCatService;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(string sortOrder,int? pageNumber, string searchString, string currentFilter,
            string status)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["Status"] = status;
            ViewData["CountTotal"] = _productService.CountTotal();
            ViewData["CountStock"] = _productService.CountByStatusWarehouse(true);
            ViewData["CountOutStock"] = _productService.CountByStatusWarehouse(false);
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
            IQueryable<Product> products = _productService.GetAllProductsWithFilter(searchString, sortOrder, status);
            return View(PaginatedList<Product>.Create(products, pageNumber ?? 1, pageSize));
        }

        [Route("create")]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM();
            ViewBag.ProductCatsDataTree = ProductCatsDataTree();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(ProductVM productVM, IFormFile file)
        {
            if(file == null)
            {
                ModelState.AddModelError("ImageUrl", "Image Is Required");
            }
            if (ModelState.IsValid)
            {
                //Get User
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                //Upload File
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = HelpersFile.GenerateFileName(file.FileName);
                var path = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images/products", fileName);
                var fileStream = new FileStream(path, FileMode.Create);
                file.CopyTo(fileStream);

                //Add more field to Product
                productVM.Product.ImageUrl = @"\images\products\" + fileName;
                productVM.Product.ApplicationUserId = user.Id;
                productVM.Product.Created = DateTime.Now;
                
                //Add Product
                if (_productService.Create(productVM.Product))
                {
                    //Get ProductCat And Child
                    List<ProductCat> productCats = _productCatService.GetAllProductCats();
                    List<ProductCat> arrProductCats = HelpersProductCat.getProductCatAndChildById(productCats, productVM.ProductCatId);

                    //Add To Pivot ProductProductCat Table
                    foreach (var productCat in arrProductCats)
                    {
                        var productProductCat = new ProductProductCat { ProductId = productVM.Product.Id, ProductCatId = productCat.Id };
                        _productProductCatService.Create(productProductCat);
                    }

                    TempData["success"] = "Create Product Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Create Product Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.ProductCatsDataTree = ProductCatsDataTree();
            return View(productVM);
        }

        [Route("edit")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Product Not Found";
                return RedirectToAction(nameof(Index));
            }
            var product = _productService.FindById(id.Value);
            if (product == null)
            {
                TempData["error"] = "Product Not Found";
                return RedirectToAction(nameof(Index));
            }
            ProductEditVM productEditVM = new ProductEditVM
            {
                Product = product,
                OldProductCatId = _productProductCatService.GetLastChildProductCat(product.Id).ProductCatId,
                NewProductCatId = _productProductCatService.GetLastChildProductCat(product.Id).ProductCatId
            };
            ViewBag.ProductCatsDataTree = ProductCatsDataTree();
            return View(productEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit")]
        public IActionResult Edit(ProductEditVM productEditVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    if (productEditVM.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productEditVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = HelpersFile.GenerateFileName(file.FileName);
                    var path = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "images/products", fileName);
                    var fileStream = new FileStream(path, FileMode.Create);
                    file.CopyTo(fileStream);
                    productEditVM.Product.ImageUrl = @"\images\products\" + fileName;
                }
                if(productEditVM.OldProductCatId != productEditVM.NewProductCatId)
                {
                    if(!UpdateProductProductCatTable(productEditVM.Product.Id, productEditVM.NewProductCatId))
                    {
                        TempData["error"] = "Update Product Fail";
                        return RedirectToAction(nameof(Index));
                    }
                }
                if (_productService.Update(productEditVM.Product))
                {
                    TempData["success"] = "Update Product Successfull";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Update Product Fail";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.ProductCatsDataTree = ProductCatsDataTree();
            return View(productEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Product Not Found";
                return RedirectToAction(nameof(Index));
            }
            var product = _productService.FindById(id.Value);
            if (product == null)
            {
                TempData["error"] = "Product Not Found";
                return RedirectToAction(nameof(Index));
            }
            if(product.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (_productService.Remove(product))
            {
                TempData["success"] = "Delete Product Successfull";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Delete Product Fail";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool UpdateProductProductCatTable(int productId ,int newProductCatId)
        {
            //Get All List Product Cat
            List<ProductCat> productCats = _productCatService.GetAllProductCats();
            //Create New List ProductCat To Add To ProductProductCatTable
            List<ProductCat> newListProductCat = HelpersProductCat.getProductCatAndChildById(productCats, newProductCatId);
            //Get Old List ProductProductCat By Product Id
            List<ProductProductCat> oldProductProductCats = _productProductCatService.GetAllProductProductCatByProductId(productId);
            //Remove
            foreach(var oldProductProductCat in oldProductProductCats)
            {
                if (!_productProductCatService.Remove(oldProductProductCat))
                {
                    return false;
                }
            }
            //Add New List ProductCat To ProductProductCatTable
            foreach (var productCat in newListProductCat)
            {
                ProductProductCat productProductCat = new ProductProductCat { ProductId = productId, ProductCatId = productCat.Id };
                if (!_productProductCatService.Create(productProductCat))
                {
                    return false;
                }
            }
            return true;
        }

        private List<ProductCatDataTree> ProductCatsDataTree(List<ProductCat>? productCats = null)
        {
            if (productCats == null)
            {
                productCats = _productCatService.GetAllProductCats();
            }
            List<ProductCatDataTree> productCatsDataTree = HelpersProductCat.getDataTree(productCats);
            return productCatsDataTree;
        }
    }
}
