using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IRequestParamReader
    {
        Task<RequestData> ReadAsync();
    }
}
