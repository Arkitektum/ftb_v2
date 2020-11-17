using System;
using System.Collections.Generic;
using System.Text;

namespace Ftb_Repositories.HttpClients
{
    public class HtmlToPDFConvertException : Exception
    {
        public string Text { get; set; }
        public int HTTPStatusCode { get; set; }
        public HtmlToPDFConvertException(string text, int hTTPStatusCode)
        {
            Text = text;
            HTTPStatusCode = hTTPStatusCode;
        }
    }
}
