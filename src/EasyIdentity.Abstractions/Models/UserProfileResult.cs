using System.Security.Claims;

namespace EasyIdentity.Models
{
    public class UserProfileResult
    {
        public string SubjectId { get; set; }

        public ClaimsIdentity Identity { get; set; }


        public bool Succeeded { get; protected set; }
        public string Error { get; protected set; }
        public string ErrorDescription { get; protected set; }

        public static UserProfileResult Success(string subjectId, ClaimsIdentity identity)
        {
            return new UserProfileResult
            {
                SubjectId = subjectId,
                Identity = identity,
                Succeeded = true,
            };
        }

        public static UserProfileResult Fail(string error, string errorDescription = null)
        {
            return new UserProfileResult
            {
                Succeeded = false,
                Error = error,
                ErrorDescription = errorDescription,
            };
        }
    }
}
