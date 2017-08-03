using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class Product
    {
        public int productId { get; set; }
        public int productQuantity { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
