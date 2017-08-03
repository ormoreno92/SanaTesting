using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;
using System.Threading.Tasks;
using ViewModels;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;

namespace SanaTestingOne.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Products _prd = new Products();
        public static List<Product> generalProducts;

        public ActionResult Home()
        {
            var rtn = GetCurrentProds();
            return View(rtn);
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddProduct(Product product)
        {
            return Json(_prd.CreateProduct(product));
        }

        [HttpGet]
        public JsonResult GetProductsStorage()
        {
            return Json(GetCurrentProds(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CleanStorage()
        {
            Session["productsTable"] = null;
            CleanFiles();
            generalProducts = new List<Product>();
            return Json("");
        }

        private void SaveDataToFile(List<Product> data, int fileType)
        {
            string path = null;
            switch (fileType)
            {
                case 3:
                    path = GetPaths() + "\\products.svc";
                    var personTexts = data.Select(p => String.Join(",", p.productId, p.Title, p.Price, p.productQuantity));
                    var joined = String.Join(Environment.NewLine, personTexts);
                    using (var fs = System.IO.File.Create(path))
                    {
                        var info = new UTF8Encoding(true).GetBytes(joined);
                        fs.Write(info, 0, info.Length);
                    }
                    break;
                case 4:
                    path = GetPaths() + "\\products.xml";
                    var xmlSerializer = new XmlSerializer(typeof(List<Product>));
                    var filestream = new StreamWriter(path);
                    xmlSerializer.Serialize(filestream, data);
                    filestream.Close();
                    break;
                case 5:
                    path = GetPaths() + "\\products.json";
                    using (var fs = System.IO.File.Create(path))
                    {
                        var info = new UTF8Encoding(true).GetBytes(JsonConvert.SerializeObject(data));
                        fs.Write(info, 0, info.Length);
                    }
                    break;
            }
        }

        private List<Product> GetFromFiles(int fileType)
        {
            var lst = new List<Product>();
            string p = null;
            p = GetPaths();
            try
            {
                switch (fileType)
                {
                    case 3:
                        p += "\\products.svc";
                        var text1 = System.IO.File.ReadAllText(p);
                        string[] lines = text1.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                        foreach (var item in lines)
                        {
                            var itm = item.Split(',');
                            lst.Add(new Product
                            {
                                productId = Convert.ToInt32(itm[0].ToString()),
                                Title = itm[1].ToString(),
                                Price = Convert.ToDecimal(itm[2].ToString()),
                                productQuantity = Convert.ToInt32(itm[3].ToString())
                            });
                        }
                        if (lst.Count > 0) return lst;
                        break;
                    case 4:
                        p += "\\products.xml";
                        var doc = XDocument.Load(p);
                        var result = doc.Descendants("Product");
                        foreach (var item in result)
                        {
                            lst.Add(new Product
                            {
                                productId = Convert.ToInt32(item.Elements("productId").FirstOrDefault().Value),
                                Title = item.Elements("Title").FirstOrDefault().Value,
                                Price = Convert.ToDecimal(item.Elements("Price").FirstOrDefault().Value),
                                productQuantity = Convert.ToInt32(item.Elements("productQuantity").FirstOrDefault().Value)
                            });
                        }
                        if (lst.Count > 0) return lst;
                        break;
                    case 5:
                        p += "\\products.json";
                        var text = System.IO.File.ReadAllText(p);
                        var rtn = JsonConvert.DeserializeObject<List<Product>>(text);
                        if (rtn != null)
                            if (rtn.Count > 0) return rtn;
                        break;
                }

            }
            catch (Exception exc)
            {
                //
            }
            return null;
        }

        private void CleanFiles()
        {
            var di = new DirectoryInfo(GetPaths());
            foreach (var file in di.GetFiles()) file.Delete();
            foreach (var dir in di.GetDirectories()) dir.Delete(true);
        }

        private string GetPaths()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var dir = Path.GetDirectoryName(path);
            dir += "\\ProductsFiles";
            return dir;
        }

        private List<Product> GetCurrentProds()
        {
            if (Session["productsTable"] != null) return Session["productsTable"] as List<Product>;
            if (generalProducts != null) if (generalProducts.Count > 0) return generalProducts;
            var filing = GetFromFiles(3);
            if (filing != null) return filing;
            filing = GetFromFiles(4);
            if (filing != null) return filing;
            filing = GetFromFiles(5);
            if (filing != null) return filing;
            generalProducts = _prd.GetProducts();
            return generalProducts;
        }

        public void ChangeStorage(List<Product> prods, int type = 0)
        {
            List<Product> products = null;
            if (prods != null) products = prods;
            products = _prd.GetProducts();
            CleanStorage();
            switch (type)
            {
                case 2:
                    Session["productsTable"] = products;
                    break;
                case 3:
                    SaveDataToFile(prods, type);
                    break;
                case 4:
                    SaveDataToFile(prods, type);
                    break;
                case 5:
                    SaveDataToFile(prods, type);
                    break;
                case 6:
                    generalProducts = products;
                    break;
                default:
                    generalProducts = products;
                    break;
            }
        }
    }
}