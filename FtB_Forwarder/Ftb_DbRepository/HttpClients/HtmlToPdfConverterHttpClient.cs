//using FtB_Common.Exceptions;
//using FtB_Common.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;

namespace Ftb_Repositories.HttpClients
{
    public class HtmlToPdfConverterHttpClient
    {
        private HttpClient _client { get; }
        private readonly IOptions<HtmlToPdfConverterSettings> _settings;
        private readonly ILogger<HtmlToPdfConverterHttpClient> _log;

        public HtmlToPdfConverterHttpClient(HttpClient httpClient
                                            , IOptions<HtmlToPdfConverterSettings> settings
                                            , ILogger<HtmlToPdfConverterHttpClient> log)
        {
            _client = httpClient;
            _settings = settings;
            _log = log;
        }

        public byte[] Get(string html)
        {
            try
            {
                byte[] PDFInbytes;
                _client.BaseAddress = new Uri(_settings.Value.UriAddress);
                html = RemoveNewLinesAndBackslash(html);

                HttpContent requestContent = new StringContent($"{{\"htmlData\":\"{html}\"}}", UTF8Encoding.UTF8);//, "application/json");
                requestContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/pdf"));
                _client.DefaultRequestHeaders.Add("User-Agent", "Arbeidsflyt/2");
                _client.DefaultRequestHeaders.Add("X-API-KEY", _settings.Value.APIKey);                

                var response = _client.PostAsync(_settings.Value.API, requestContent).Result;
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
