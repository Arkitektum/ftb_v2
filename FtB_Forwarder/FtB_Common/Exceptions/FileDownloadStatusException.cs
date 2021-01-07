using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Exceptions
{
    public class FileDownloadStatusException : Exception
    {
        public string Text { get; set; }
        public FileDownloadStatusException(string text)
        {
            Text = text;
        }
    }
}
