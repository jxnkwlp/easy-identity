using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

/// <summary>
///  Device code manager class
/// </summary>
public interface IDeviceCodeManager
{
    /// <summary>
    ///  request an new device code 
    /// </summary> 
    Task<DeviceCodeData> RequestAsync(RequestData requestData, RequestValidationResult validationResult);

    /// <summary>
    ///  Verify that 'deviceCode' is valid. 
    /// </summary>
    /// <remarks>
    ///  Returns false if the code has expired or does not exist
    /// </remarks>
    Task<bool> ValidateDeviceCodeAsync(string deviceCode, Client client);

    /// <summary>
    ///  Find 'deviceCode' by 'userCode'
    /// </summary>
    /// <remarks>
    ///  Returns null if the code does not exist.
    /// </remarks>
    Task<string> FindDeviceCodeAsync(string userCode, Client client = null);

    Task<string> FindSubjectAsync(string deviceCode, Client client);

    Task<bool> CheckGrantedAsync(string deviceCode, Client client);

    Task GrantAsync(string deviceCode, Client client, ClaimsPrincipal principal);

    Task RejectAsync(string deviceCode, Client client, ClaimsPrincipal principal);

}
