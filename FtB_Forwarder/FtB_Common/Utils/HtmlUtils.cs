using iText.Html2pdf;
using System;
using System.IO;

namespace FtB_Common.Utils
{
    public class HtmlUtils
    {
        public static string GetTextFromTemplate(string htmlTemplatePath)
        {
            string htmlBody = "";
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.ToUpper().Contains("FTB_FORMLOGIC"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(htmlTemplatePath))
                    {
                        if (stream == null)
                        {
                            throw new Exception($"The resource {htmlTemplatePath} was not loaded properly.");
                        }

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            htmlBody = reader.ReadToEnd();
                        }
                    }
                }
            }

            return htmlBody;
        }

        //TODO: Add copyright notice for iText7 AGPL license
        public static byte[] GetPDFFromHTML(string html)
        {
            try
            {
                string LogoResourceBase = "FtB_Common.Images";
                byte[] PDFInbytes;
                using (var ms = new MemoryStream())
                {
                    ConverterProperties properties = new ConverterProperties();
                    properties.SetBaseUri(LogoResourceBase);
                    HtmlConverter.ConvertToPdf(html, ms, properties);
                    PDFInbytes = ms.ToArray();
                    ms.Dispose();
                }

                return PDFInbytes;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
