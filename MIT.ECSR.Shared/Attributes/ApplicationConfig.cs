using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Shared.Attributes
{
    public class ApplicationConfig
    {
        public string WebURL { get; set; }
        public string APIURL { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpired { get; set; }
        public string DefaultPassword { get; set; }
    }
}
