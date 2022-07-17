using System;
using System.Collections.Generic;

namespace EasyIdentity.Models
{
    public class ResponseDescriptor
    {
        public bool Succeeded => string.IsNullOrEmpty(Error);

        public string Error { get; }

        public string ErrorDescription { get; }

        public string HttpLocation { get; }

        public Dictionary<string, object> Data { get; }

        public RequestData RequestData { get; }

        public ResponseDescriptor(RequestData requestData, string httpLocation)
        {
            RequestData = requestData;
            HttpLocation = httpLocation;
        }

        public ResponseDescriptor(RequestData requestData, Dictionary<string, object> data)
        {
            RequestData = requestData;
            Data = data;
        }

        public ResponseDescriptor(RequestData requestData, string error, string errorDescription)
        {
            RequestData = requestData;
            Error = error;
            ErrorDescription = errorDescription;
        }

        public ResponseDescriptor(RequestData requestData, Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            RequestData = requestData ?? throw new ArgumentNullException(nameof(requestData));
            Error = exception.Message;
            ErrorDescription = exception.InnerException?.Message ?? string.Empty;
        }
    }
}
