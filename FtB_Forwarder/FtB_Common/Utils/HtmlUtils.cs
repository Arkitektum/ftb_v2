using FtB_Common.Exceptions;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace FtB_Common.Utils
{
    public class HtmlUtils : IHtmlUtils
    {
        private readonly ILogger<HtmlUtils> _log;
        private readonly IOptions<HtmlAndPdfGeneratorSettings> _htmlAndPdfGeneratorSettings;

        public HtmlUtils(IOptions<HtmlAndPdfGeneratorSettings> htmlAndPdfGeneratorSettings, ILogger<HtmlUtils> log)
        {
            _htmlAndPdfGeneratorSettings = htmlAndPdfGeneratorSettings;
            _log = log;
        }
        public string GetHtmlFromTemplate(string htmlTemplatePath)
        {
            string htmlBody = "";
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.ToUpper().Contains(_htmlAndPdfGeneratorSettings.Value.HtmlTemplateAssembly.ToUpper()))       // "FTB_FORMLOGIC"))
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
                    break;
                }
            }

            return htmlBody;
        }

        public byte[] GetPDFFromHTML(string html)
        {
            try
            {
                byte[] PDFInbytes;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_htmlAndPdfGeneratorSettings.Value.UriAddress);
                    html = RemoveNewLinesAndBackslash(html);

                    HttpContent requestContent = new StringContent($"{{\"htmlData\":\"{html}\"}}", UTF8Encoding.UTF8);//, "application/json");
                    requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/pdf"));
                    client.DefaultRequestHeaders.Add("User-Agent", "Arbeidsflyt/2");

                    var response = client.PostAsync(_htmlAndPdfGeneratorSettings.Value.API, requestContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        _log.LogDebug($"{GetType().Name}. Successfully converted the HTML: {html}");
                        PDFInbytes = response.Content.ReadAsByteArrayAsync().Result;
                    }
                    else
                    {
                        _log.LogError($"{GetType().Name}. Failure during converting the HTML: {html}");
                        throw new HtmlToPDFConvertException($"Error converting html: {System.Environment.NewLine}{html}", (int)response.StatusCode);
                    }

                }

                return PDFInbytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string RemoveNewLinesAndBackslash(string input)
        {
            return input.Replace($"\r\n", "").Replace($"\"", "'");
        }
    }

}
