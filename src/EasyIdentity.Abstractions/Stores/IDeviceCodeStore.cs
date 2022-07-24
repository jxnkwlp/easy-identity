using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

/// <summary> 
///  An simple store interface for DeviceCode
/// </summary> 
public interface IDeviceCodeStore<TDeviceCode> where TDeviceCode : class
{
    Task CreateAsync(DeviceCodeData data, string clientId, DateTime expiration, CancellationToken cancellationToken = default);

    Task RemoveAsync(string deviceCode, CancellationToken cancellationToken = default);

    Task<TDeviceCode> FindAsync(string deviceCode, CancellationToken cancellationToken = default);

    Task<TDeviceCode> FindByUserCodeAsync(string userCode, CancellationToken cancellationToken = default);

    Task<string> GetUserCodeAsync(TDeviceCode deviceCode, CancellationToken cancellationToken = default);

    Task<string> GetDeviceCodeAsync(TDeviceCode deviceCode, CancellationToken cancellationToken = default);

    Task<string> GetSubjectAsync(TDeviceCode deviceCode, CancellationToken cancellationToken = default);

    Task<string> GetClientIdAsync(TDeviceCode deviceCode, CancellationToken cancellationToken = default);

    Task<bool?> GetGrantedAsync(TDeviceCode deviceCode, CancellationToken cancellationToken = default);

    Task<DateTime> GetExpirationAsync(TDeviceCode deviceCode, CancellationToken cancellationToken = default);

    Task SetGrantedAsync(TDeviceCode deviceCode, bool granted, CancellationToken cancellationToken = default);

    Task SetSubjectAsync(TDeviceCode deviceCode, string subject, CancellationToken cancellationToken = default);

}
