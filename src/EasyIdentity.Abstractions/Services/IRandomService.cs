using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IRandomService
{
    ValueTask<string> RandomAsync(int length, bool capital, bool lowercase, bool digital);
}
