using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpProject
{
    public class Product
    {

        public static List<Product> products;
        private string _category;

        public string Name { get; set; }
        public string Category
        {
            get
            {
                return _category;
            }

            set
            {
                switch (value)
                {
                    case "1":
                        _category = "Desktop";
                        break;
                    case "2":
                        _category = "Laptop";
                        break;
                    case "3":
                        _category = "Network";
                        break;
                    case "4":
                        _category = "Printer";
                        break;
                    case "5":
                        _category = "Screen";
                        break;
                    case "6":
                        _category = "Acessories";
                        break;
                    case "7":
                        _category = "Computer parts";
                        break;
                }
            }
        }
        public string Description { get; set; }

        private int _price;
        public int Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value > 0)
                {
                    _price = value;
                }
            }
        }

        public Product(string name,/*string category,*/ string description, int price)
        {
            Name = name;
            //Category = category;
            Description = description;
            Price = price;
        }
    }
}
