using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Using for catching IOException
using System.IO;
// Using for inter-processing communication
using System.IO.Pipes;
// Using for deserialize the JSON string
using Newtonsoft.Json;

namespace Interprocess
{
    // Server class
    class Program
    {
        static void Main(string[] args)
        {
            // Using ReceivedSingleMessage object to access class methods
            Message message = new Message();
            // Write this line to acknowledge to the user that you are a server
            Console.WriteLine("Starting server.");
            bool hasEnded = false, hasSelected = false;
            // Continue loop if server wants to see another client
            do
            {
                // Using method allows to dispose NamedPipeServerStream object.
                // The pipe communication direction will be in two-ways and must be connected with a pipe name and read as message
                using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("pipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message))
                {
                    try
                    {
                        // Waiting for the client connection
                        Console.WriteLine("Waiting for client connection...");
                        pipeServer.WaitForConnection();
                        // Write a response to the client
                        Console.Write("Client connected. Send greetings to client: ");
                        string messageForClient = Console.ReadLine();
                        // Encode the response into block of bytes and write to the current stream for the client
                        byte[] messageByte = Encoding.UTF8.GetBytes(messageForClient);
                        pipeServer.Write(messageByte, 0, messageByte.Length);
                        // Receives JSON string from the client, receives order
                        string response = message.ProcessSingleReceivedMessage(pipeServer);
                        // Deserialize this string into a list of order
                        List<Order> order = JsonConvert.DeserializeObject<List<Order>>(response);
                        // Initialise count to 1 as this starts with first order if any
                        int count = 1;
                        // If the list is not empty
                        if (order.Count > 0)
                        {
                            // Display the list of order in details
                            foreach (Order product in order)
                            {
                                Console.WriteLine("\r");
                                Console.WriteLine("Order Details #" + count);
                                product.printOrder();
                                // Increment count by 1 to see if there are any more orders
                                count++;
                            }
                        }
                        // If list is empty, display message
                        else
                        {
                            Console.WriteLine("There is no order.");
                        }
                        // Wait until server read all of the orders
                        Console.ReadLine();
                        // Check if the server wants to see another client available
                        // Continue this loop until user selected yes or no
                        do
                        {
                            Console.WriteLine("Do you want to see another client?");
                            response = Console.ReadLine();
                            string answer = response.ToLower();
                            // Enter response and check if answer is yes or no in lower case format
                            if (answer == "yes" || answer == "y")
                            {
                                hasSelected = true;
                                hasEnded = false;
                            }
                            else if (answer == "no" || answer == "n")
                            {
                                hasSelected = true;
                                hasEnded = true;
                            }
                            else
                            {
                                hasSelected = false;
                            }
                        }
                        while (!hasSelected);
                    }
                    // If client is disconnected, check if there are other clients
                    catch (IOException error)
                    {
                        hasEnded = false;
                        Console.WriteLine("Client has been disconnected. Error by " + error.Message);
                    }
                }
            }
            while (!hasEnded);
        }
    }
}
