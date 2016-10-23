using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.ApiServices.ShoppingList.Models
{
    public class Item : BaseEntity
    {
        public int ItemID
        {
            get { return Id; }
            set { Id = value; }
        }

        public string CustomerID { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
