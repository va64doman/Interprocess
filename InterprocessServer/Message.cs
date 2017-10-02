using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Using for inter-processing communication
using System.IO.Pipes;

namespace Interprocess
{
    public class Message
    {
        // Convert block of bytes into string
        public string ProcessSingleReceivedMessage(NamedPipeServerStream pipeServer)
        {
            // Initialise string builder
            StringBuilder messageBuilder = new StringBuilder();
            // Set messageChunk to empty string
            string messageChunk = string.Empty;
            // Set byte array size to 5
            byte[] messageBuffer = new byte[5];
            // Continue while message from client is completely converted into string
            do
            {
                // Read a block of bytes and write as message
                pipeServer.Read(messageBuffer, 0, messageBuffer.Length);
                // Convert bytes into string to set messageChunk
                messageChunk = Encoding.UTF8.GetString(messageBuffer);
                // Copied string to string builder
                messageBuilder.Append(messageChunk);
                // Set new buffer by the byte array and set length as 5
                messageBuffer = new byte[messageBuffer.Length];
            }
            while (!pipeServer.IsMessageComplete);
            // Return this string builder
            return messageBuilder.ToString();
        }
    }
}
