using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Using for inter-processing communication
using System.IO.Pipes;
// Using for serialise list into string
using Newtonsoft.Json;

namespace Interprocess
{
    public class Message
    {
        // Client received message from server and delivering order to server
        // Input is set to private for handling integer
        private int input;
        // Read a block of bytes from server and convert into string for client to read
        public string ProcessSingleReceivedMessage(NamedPipeClientStream pipeClient)
        {
            // Initialise string builder
            StringBuilder messageBuilder = new StringBuilder();
            // Set messageChunk to empty string as read-only
            string messageChunk = string.Empty;
            // Set messageBuffer to 5 block of bytes
            byte[] messageBuffer = new byte[5];
            // Continue until data becomes readable to user
            do
            {
                // Read a block of bytes and write as message
                pipeClient.Read(messageBuffer, 0, messageBuffer.Length);
                // Convert bytes into string to set messageChunk
                messageChunk = Encoding.UTF8.GetString(messageBuffer);
                // Copied string to string builder
                messageBuilder.Append(messageChunk);
                // Set new buffer by the byte array and set length as 5
                messageBuffer = new byte[messageBuffer.Length];
            }
            while (!pipeClient.IsMessageComplete);
            // Return the string as string builder converted to string
            return messageBuilder.ToString();
        }
        // Allows user to create one or more orders to the server
        public string GetOrder(NamedPipeClientStream pipeClient, string question)
        {
            // Initialise list to store products
            List<Order> order = new List<Order>();
            // Set response, answer and serialise to string
            string response, answer, serialise;
            // Set hasEnded and hasSelected to false
            bool hasEnded = false, hasSelected = false;
            // Continue this while the user wants to keep ordering
            do
            {
                // Display the question to the client
                Console.WriteLine(question);
                // Set name, product and address as string
                string name, product, address;
                // Set quantity as integer
                int quantity;
                // Continue while name is empty
                do
                {
                    // Allow the client to enter your name if there are many sharing with one application
                    Console.Write("Enter your name: ");
                    name = Console.ReadLine();
                }
                while (String.IsNullOrWhiteSpace(name));
                // Continue while product is empty
                do
                {
                    // Enter the product that the user wants to buy
                    Console.Write("Enter the name of product: ");
                    product = Console.ReadLine();
                }
                while (String.IsNullOrWhiteSpace(product));
                // Continue while quantity is less than 1
                do
                {
                    // Enter the quantity of the product
                    Console.Write("Enter the number of quantity for this product: ");
                    quantity = handleInt();
                }
                while (quantity < 1);
                // Continue while address is empty
                do
                {
                    // Enter the user's home address or email address
                    Console.Write("Enter your home address or email address: ");
                    address = Console.ReadLine();
                }
                while (String.IsNullOrWhiteSpace(address));
                // Order object initialiser
                // Set new order object using user inputs
                Order customerOrder = new Order();
                customerOrder.productName = product;
                customerOrder.productQuantity = quantity;
                customerOrder.customerName = name;
                customerOrder.customerAddress = address;
                // Add order object to order's list
                Console.WriteLine("Order is successfully added.");
                order.Add(customerOrder);
                // Continue this loop until the user selected yes or no
                do
                {
                    // Display message if the user wants to add more orders
                    Console.WriteLine("Do you want to keep going? Yes(Y) or No(N)");
                    // User enters yes or no
                    response = Console.ReadLine();
                    // Set answer as response in lower case format
                    answer = response.ToLower();
                    // If yes, continue adding order
                    if (answer == "yes" || answer == "y")
                    {
                        hasSelected = true;
                        hasEnded = false;
                    }
                    // If no, end client
                    else if(answer == "no" || answer == "n")
                    {
                        hasSelected = true;
                        hasEnded = true;
                    }
                    // If neither, continue this while the user has selected yes or no
                    else
                    {
                        hasSelected = false;
                    }
                }
                while (!hasSelected);
            }
            while (!hasEnded);
            // Convert the list of order into JSON string for server
            serialise = JsonConvert.SerializeObject(order);
            // Encodes serialise into stream of bytes
            byte[] responseByte = Encoding.UTF8.GetBytes(serialise);
            // Write the block of serialised bytes to the server
            pipeClient.Write(responseByte, 0, responseByte.Length);
            // Set jsonString to convert block of bytes into JSON string to the server
            string jsonString = ProcessSingleReceivedMessage(pipeClient);
            // Return jsonString
            return jsonString;
        }
        // Handling integer input
        private int handleInt()
        {
            // Set loop to true
            bool loop = true;
            // Continue looping
            while (loop)
            {
                // If user input is an integer, break the loop and return this integer
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    break;
                }
                // Else, repeat again
                else
                {
                    Console.WriteLine("Try again.");
                }
            }
            return input;
        }
    }
}
