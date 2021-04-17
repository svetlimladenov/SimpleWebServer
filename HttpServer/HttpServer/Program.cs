using System;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            await AppBuilder.Start();
        }
    }
}
