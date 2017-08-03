using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Db;

namespace Logic
{
    public class Products
    {
        private readonly SanaTestingDbEntities _db = new SanaTestingDbEntities();
        public List<Product> generalProducts;

        public List<Product> GetProducts()
        {
            if (generalProducts != null) if (generalProducts.Count > 0) return generalProducts;

            var products = new List<Product>();
            try
            {
                var list = _db.Products.ToList();
                foreach (var product in list) products.Add(new Product
                {
                    Price = product.Products_productPrice,
                    productId = product.Products_productId,
                    Title = product.Products_productName,
                    productQuantity = product.Products_productQuantity
                });
            }
            catch (Exception ex)
            {
                //                    
            }
            generalProducts = products;
            return products;
        }

        public bool CreateProduct(Product product)
        {
            try
            {
                _db.Products.Add(new Db.Products
                {
                    Products_productName = product.Title,
                    Products_productPrice = product.Price,
                    Products_productQuantity = product.productQuantity,
                    Products_productDescription = product.Title
                });
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //                    
            }
            return false;
        }
    }
}
