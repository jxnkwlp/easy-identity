namespace EasyIdentity.Models
{
    public class GrantTypeHandleResult
    {
        public string ResponseLocation { get; set; }
        public TokenResponseData ResponseData { get; set; }
        
        public string GrantType { get; set; }

        public bool HasError => !string.IsNullOrEmpty(Error);
        public string Error { get; protected set; }
        public string ErrorDescription { get; protected set; }

        public void SetError(string error, string errorDescription = null)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
