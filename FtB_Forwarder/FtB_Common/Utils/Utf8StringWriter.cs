using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FtB_Common.Utils
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}
