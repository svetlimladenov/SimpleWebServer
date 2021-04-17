using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHttpServer server = new HttpServer();
            server.Start();
        }

        public interface IHttpServer
        {
            void Start();

            void Stop();
        }

        public class HttpServer : IHttpServer
        {
            private bool isAlive;
            private TcpListener tcpListener;

            public HttpServer()
            {
                this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);  // localhost
            }

            public void Start()
            {
                this.isAlive = true;
                this.tcpListener.Start();

                while (this.isAlive)
                {
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    var requst = Read(stream);
                    Console.WriteLine(requst);

                    var response = "Hello World";
                    Respond(stream, response);
                }
            }

            private static void Respond(NetworkStream stream, string responseText)
            {

                var responseBytes = Encoding.UTF8.GetBytes(
                                        "HTTP/1.0 200 OK" + Environment.NewLine +
                                        $"Content-Length: {responseText.Length}" + Environment.NewLine +
                                        Environment.NewLine +
                                        responseText
                                        );

                stream.Write(responseBytes);
            }

            private static string Read(NetworkStream stream)
            {
                var buffer = new byte[10240];
                int readLenght = stream.Read(buffer, 0, buffer.Length);
                string requestText = Encoding.UTF8.GetString(buffer, 0, readLenght);
                return requestText;
            }

            public void Stop()
            {
                this.isAlive = false;
            }
        }

        public class Service : IHostedService
        {
            public async Task StartAsync(CancellationToken cancellationToken)
            {
            }

            public async Task StopAsync(CancellationToken cancellationToken)
            {
            }
        }
    }
}
