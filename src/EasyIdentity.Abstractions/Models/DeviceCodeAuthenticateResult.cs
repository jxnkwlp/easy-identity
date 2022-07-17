namespace EasyIdentity.Models
{
    public class DeviceCodeAuthenticateResult
    {
        public bool Granted => !string.IsNullOrEmpty(Subject);

        public string Subject { get; protected set; }

        public static DeviceCodeAuthenticateResult Grant(string subject)
        {
            return new DeviceCodeAuthenticateResult { Subject = subject };
        }

        public static DeviceCodeAuthenticateResult Reject(string subject)
        {
            return new DeviceCodeAuthenticateResult { Subject = null };
        }
    }
}
