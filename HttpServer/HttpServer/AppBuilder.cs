using System;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public static class AppBuilder
    {
        public static async Task Start()
        {
            IHttpServer server = new HttpServer();
            var hostedService = new Service(server);
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await hostedService.StartAsync(tokenSource.Token);
        }
    }
}
