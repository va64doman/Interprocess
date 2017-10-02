using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Using for catching IOException
using System.IO;
// Using for inter-processing communication
using System.IO.Pipes;

namespace Interprocess
{
    // Client class
    class Program
    {
        static void Main(string[] args)
        {
            // Using ReceivedSingleMessage object to access class methods
            Message message = new Message();
            // Write this line to user that the user is a client
            Console.WriteLine("Starting client.");
            // Using method allows to dispose NamedPipeClientStream object.
            // The pipe communication direction will be in two-ways and must be connected with a pipe name
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "pipe", PipeDirection.InOut))
            {
                try
                {
                    // Waiting for the server to be connected
                    Console.WriteLine("Attempting to connect to pipe server...");
                    pipeClient.Connect();
                    // Display the number of servers connected to the client
                    Console.WriteLine("There are currently {0} pipe server instances open.", pipeClient.NumberOfServerInstances);
                    Console.WriteLine("Connected to server.");
                    // The read mode for pipe client is in stream of messages
                    pipeClient.ReadMode = PipeTransmissionMode.Message;
                    // Get message from pipe server's input
                    string messageForServer = message.ProcessSingleReceivedMessage(pipeClient);
                    Console.WriteLine("The server is saying {0}", messageForServer);
                    // Transfer Json string to the server
                    string response = message.GetOrder(pipeClient, "What do you want to order?");
                }
                catch(IOException error)
                {
                    // If server disconnected, end the client
                    Console.WriteLine("Server is disconnected. Error by {0}. Press enter to dismiss.", error.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}
