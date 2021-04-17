using System.Threading.Tasks;

namespace HttpServer
{
    public interface IHttpServer
    {
        Task Start();

        Task Stop();
    }
}
