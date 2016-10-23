using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.ApiServices.ShoppingList.Models
{
    public class Product : BaseEntity
    {
        public int ProductID { get { return Id; } set { Id = value; } }
        public string Type { get; set; }

        public List<Item> Items { get; set; }
    }
}
