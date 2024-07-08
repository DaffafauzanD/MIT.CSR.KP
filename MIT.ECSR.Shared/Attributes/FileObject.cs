using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Shared.Attributes
{
    public class FileObject
    {
        public string Filename { get; set; }
        public string Base64 { get; set; }
    }
    public class FileObjectPath
    {
        public string FilePath { get; set; }
        public string Filename { get; set; }
        public string Base64 { get; set; }
    }
}
