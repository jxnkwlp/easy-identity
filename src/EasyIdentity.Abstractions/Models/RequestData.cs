using System.Collections.Generic;

namespace EasyIdentity.Models
{
    public class RequestData
    {
        public Dictionary<string, string> Data { get; }

        public RequestData(Dictionary<string, string> data)
        {
            Data = data;
        }

        public string this[string key]
        {
            get
            {
                string value;
                Data.TryGetValue(key, out value);
                return value;
            }
        }
    }
}
