using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interprocess
{
    // Order's details
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
        // Print the order's details
        public void printOrder()
        {
            Console.WriteLine("Customer Name: " + customerName);
            Console.WriteLine("Customer Address: " + customerAddress);
            Console.WriteLine("Product Name: " + productName);
            Console.WriteLine("Quantity: " + productQuantity);
        }
    }
}
