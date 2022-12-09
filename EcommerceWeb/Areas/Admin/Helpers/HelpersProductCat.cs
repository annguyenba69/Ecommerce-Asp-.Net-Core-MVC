using Ecommerce.Models;

namespace EcommerceWeb.Areas.Admin.Helpers
{
    public class HelpersProductCat
    {
        
        public static List<ProductCatDataTree> getDataTree(List<ProductCat> productCats,
            int parentId = 0, int level = 0)
        {
            var listProductCatDataTree = new List<ProductCatDataTree>();
            for (int key = 0; key < productCats.Count; key++)
            {
                if (productCats[key].ParentId == parentId)
                {
                    ProductCatDataTree productCatDataTree = new ProductCatDataTree
                    {
                        ProductCat = productCats[key],
                        Level = level
                    };
                    listProductCatDataTree.Add(productCatDataTree);
                    var child = getDataTree(productCats, productCatDataTree.ProductCat.Id, level + 1);
                    listProductCatDataTree.AddRange(child);
                }
            }
            return listProductCatDataTree;
        }

        //Hàm kiểm tra xem danh mục đó có danh mục con hay không
        public static bool checkChild(List<ProductCat> productCats, int id)
        {
            foreach(var productCat in productCats)
            {
                if(productCat.ParentId == id)
                {
                    return true;
                    break;
                }
            }
            return false;
        }

        //Hàm lấy danh mục theo Id và tất cả danh mục con của danh mục đó
        public static List<ProductCat> getProductCatAndChildById(List<ProductCat> productCats, int id)
        {
            List<ProductCat> arrProductCats = new List<ProductCat>();
            foreach(var product in productCats)
            {
                if(product.Id == id)
                {
                    arrProductCats.Add(product);
                    productCats.Remove(product);
                    foreach(var parentProduct in productCats)
                    {
                        if(parentProduct.Id == product.ParentId)
                        {
                            arrProductCats.Add(parentProduct);
                        }
                    }
                    break;
                }
            }
            return arrProductCats;
        }

        //Hàm lấy danh mục con nhỏ nhất

    }
}

