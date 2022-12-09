using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Service;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EcommerceWeb.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ProductProductCatService _productProductCatService;
        private readonly ProductCatService _productCatService;
        private readonly ApplicationDbContext _db;
        public ProductController(ProductService productService, ProductProductCatService productProductCatService,
            ProductCatService productCatService, ApplicationDbContext db)
        {
            _productService = productService;
            _productProductCatService = productProductCatService;
            _productCatService = productCatService; 
            _db = db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index(string sortOrder, int? pageNumber, string searchString,
            string currentFilter, string status, int? productCatId)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["Status"] = status;
            int pageSize = 9;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["Title"] = _productCatService.GetNameProductCat(productCatId);
            ViewData["CountTotalByProductCatId"] = _productProductCatService.CountProductsBelongToProductCatId(productCatId);
            IQueryable<Product> products = _productService.GetAllProductsWithFilter(searchString, sortOrder, status, productCatId);
            return View(PaginatedList<Product>.Create(products, pageNumber ?? 1, pageSize));
        }

        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var product = _productService.FindById(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}
