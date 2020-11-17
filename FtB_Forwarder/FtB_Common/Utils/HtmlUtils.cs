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
        private readonly IOptions<HtmlUtilSettings> _htmlUtilSettings;

        public HtmlUtils(IOptions<HtmlUtilSettings> htmlUtilSettings
                        , ILogger<HtmlUtils> log)
        {
            _htmlUtilSettings = htmlUtilSettings;
            _log = log;
        }
        public string GetHtmlFromTemplate(string htmlTemplatePath)
        {
            string htmlBody = "";
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.ToUpper().Contains(_htmlUtilSettings.Value.HtmlTemplateAssembly.ToUpper()))       // "FTB_FORMLOGIC"))
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
    }
}
