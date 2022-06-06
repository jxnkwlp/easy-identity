//using System;
//using System.Collections.Generic;

//namespace EasyIdentity.Models
//{
//    [Obsolete]
//    public class RequestParsedResult
//    {
//        public Dictionary<string, string> Values { get; }

//        public RequestParsedResult(Dictionary<string, string> values)
//        {
//            Values = values;
//        }

//        public string this[string key]
//        {
//            get
//            {
//                string value;
//                Values.TryGetValue(key, out value);
//                return value;
//            }
//        }
//    }
//}
