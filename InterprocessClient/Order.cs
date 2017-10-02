using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interprocess
{
    // Order details
    class Order
    {
        // Get and set product's name
        public string productName { get; set; }
        // Get and set quantity
        public int productQuantity { get; set; }
        // Get and set customer's name
        public string customerName { get; set; }
        // Get and set customer's address
        public string customerAddress { get; set; }
    }
}
