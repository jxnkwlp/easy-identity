using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IResponseWriter
    {
        Task WriteAsync(ResponseDescriptor descriptor);
    }
}
