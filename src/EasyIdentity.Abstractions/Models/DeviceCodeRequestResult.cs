namespace EasyIdentity.Models
{
    public class DeviceCodeRequestResult
    {
        public string DeviceCode { get; set; }
        public string UserCode { get; set; }
        public string VerificationUri { get; set; }
        public int Interval { get; set; }
        public int ExpiresIn { get; set; }
    }
}
