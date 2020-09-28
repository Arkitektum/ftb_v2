using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Exceptions
{
    public class TableStorageConcurrentException : Exception
    {
        public string Text { get; set; }
        public int HTTPStatusCode { get; private set; }
        public TableStorageConcurrentException(string text, int hTTPStatusCode)
        {
            Text = text;
            HTTPStatusCode = hTTPStatusCode;
        }
    }
}
