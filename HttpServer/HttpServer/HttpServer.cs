using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public class HttpServer : IHttpServer
    {
        private bool isAlive;
        private TcpListener tcpListener;

        public HttpServer()
            : this("127.0.0.1", 80)
        {
        }

        public HttpServer(string address, int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Parse(address), port);
        }

        public async Task Start()
        {
            this.isAlive = true;
            this.tcpListener.Start();

            while (this.isAlive)
            {
                var client = await this.tcpListener.AcceptTcpClientAsync();

                _ = Task.Run(() => HandleRequest(client)); // its working in parallel only when we are using different browsers
            }
        }

        private async Task HandleRequest(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            var requst = await ReadAsync(stream);
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(requst);
            var response = "Hello World";
            await Respond(stream, response);
        }

        private async Task Respond(NetworkStream stream, string responseText)
        {
            var responseBytes = Encoding.UTF8.GetBytes(
                                    "HTTP/1.0 200 OK" + Environment.NewLine +
                                    $"Content-Length: {responseText.Length}" + Environment.NewLine +
                                    Environment.NewLine +
                                    responseText
                                    );

            await stream.WriteAsync(responseBytes);
        }

        private async Task<string> ReadAsync(NetworkStream stream)
        {
            var buffer = new byte[10240];
            int readLenght = await stream.ReadAsync(buffer, 0, buffer.Length);
            string requestText = Encoding.UTF8.GetString(buffer, 0, readLenght);
            return requestText;
        }

        public Task Stop()
        {
            this.isAlive = false;
            return Task.CompletedTask;
        }
    } 
}
