using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

/// <summary>
///  The requeset params reader
/// </summary>
public interface IRequestParamReader
{
    Task<RequestData> ReadAsync();
}
