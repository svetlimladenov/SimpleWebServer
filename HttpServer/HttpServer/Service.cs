using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public class Service : IHostedService
    {
        private readonly IHttpServer server;

        public Service(IHttpServer server)
        {
            this.server = server;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.server.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.server.Stop();
            return Task.CompletedTask;
        }
    }
}
