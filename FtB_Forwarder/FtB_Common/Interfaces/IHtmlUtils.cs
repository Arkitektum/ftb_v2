using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Interfaces
{
    public interface IHtmlUtils
    {
        public byte[] GetPDFFromHTML(string html);
        string GetHtmlFromTemplate(string htmlTemplatePath);
    }
}
