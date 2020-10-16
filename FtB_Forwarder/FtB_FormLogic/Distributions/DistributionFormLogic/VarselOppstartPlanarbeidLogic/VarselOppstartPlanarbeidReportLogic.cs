using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_MessageManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Report)]
    public class VarselOppstartPlanarbeidReportLogic : DistributionReportLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        private readonly IBlobOperations _blobOperations;

        public VarselOppstartPlanarbeidReportLogic(IFormDataRepo repo,
                                                   ITableStorage tableStorage,
                                                   ILogger<VarselOppstartPlanarbeidReportLogic> log,
                                                   IBlobOperations blobOperations,
                                                   INotificationAdapter notificationAdapter)
            : base(repo, tableStorage, log, notificationAdapter)
        {
            _blobOperations = blobOperations;
        }
        public override AltinnReceiver GetReceiver()
        {
            return new AltinnReceiver()
            {
                Id = FormData.forslagsstiller.organisasjonsnummer,
                Type = AltinnReceiverType.Foretak
            };
        }
        private string GetContactPerson()
        {
            StringBuilder builder = new StringBuilder();
            if (!base.FormData.forslagsstiller.kontaktperson.navn.IsNullOrEmpty())
            {

                builder.Append(base.FormData.forslagsstiller.kontaktperson.navn);
                if (!base.FormData.forslagsstiller.kontaktperson.mobilnummer.IsNullOrEmpty())
                {
                    builder.Append("<br> Mobil: ");
                    builder.Append(base.FormData.forslagsstiller.kontaktperson.mobilnummer);
                }
                if (!base.FormData.forslagsstiller.kontaktperson.telefonnummer.IsNullOrEmpty())
                {
                    builder.Append("<br> Telefon: ");
                    builder.Append(base.FormData.forslagsstiller.kontaktperson.telefonnummer);
                }
                if (!base.FormData.forslagsstiller.kontaktperson.epost.IsNullOrEmpty())
                {
                    builder.Append("<br> E-post: ");
                    builder.Append(base.FormData.forslagsstiller.kontaktperson.epost);
                }
            }

            else if (!base.FormData.forslagsstiller.navn.IsNullOrEmpty())
            {
                builder.Append(base.FormData.forslagsstiller.navn);

                if (!base.FormData.forslagsstiller.telefon.IsNullOrEmpty())
                {
                    builder.Append("<br> Telefon: ");
                    builder.Append(base.FormData.forslagsstiller.telefon);
                }
                if (!base.FormData.forslagsstiller.epost.IsNullOrEmpty())
                {
                    builder.Append("<br> E-post: ");
                    builder.Append(base.FormData.forslagsstiller.epost);
                }
            }
            //_log.LogDebug($"{GetType().Name}. Getting contact person: {builder.ToString()}.");

            return builder.ToString();
        }

        protected override MessageDataType GetSubmitterReceiptMessage(string archiveReference)
        {
            try
            {
                string adresse = base.FormData.eiendomByggested.First().adresse.adresselinje1;
                string planNavn = base.FormData.planforslag.plannavn == null ? "" : base.FormData.planforslag.plannavn;
                string byggested = adresse != null && adresse.Trim().Length > 0 ? $"{adresse}, {planNavn}" : $"{planNavn}";
                string kontaktperson = GetContactPerson();

                //Get html embedded resource file
                string htmlBody = "";
                //var HtmlBodyTemplate = "VarselOppstartPlanarbeidReceiptMessageBody.html";
                var HtmlBodyTemplate = "FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Report.VarselOppstartPlanarbeidReceiptMessageBody.html";

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name.ToUpper().Contains("FTB_FORMLOGIC"))
                    {
                        _log.LogDebug($"{GetType().Name}. Found assembly: {assembly.FullName}");
                        using (Stream stream = assembly.GetManifestResourceStream(HtmlBodyTemplate))
                        {
                            if (stream == null)
                            {
                                throw new Exception($"The resource {HtmlBodyTemplate} was not loaded properly.");
                            }

                            using (StreamReader reader = new StreamReader(stream))
                            {
                                htmlBody = reader.ReadToEnd();
                            }
                        }
                    }
                }
                //_log.LogDebug($"{GetType().Name}: htmlBody: {htmlBody}");
                //TODO: Add Logo?
                //string LogoResourceName = "KommIT.FIKS.AdapterAltinnSvarUt.Content.images.dibk_logo.png";
                htmlBody = htmlBody.Replace("<byggested/>", byggested);
                htmlBody = htmlBody.Replace("<kontaktperson/>", kontaktperson);
                htmlBody = htmlBody.Replace("<arkivReferanse/>", archiveReference.ToUpper());


                var mess = new MessageDataType()
                {
                    MessageTitle = $"Melding: Kvittering - varsel for oppstart av reguleringsplanarbeid, {byggested}",
                    MessageSummary = "Trykk på vedleggene under for å laste ned varselet og kvittering med liste over hvilke berørte parter som har blitt varslet",
                    MessageBody = htmlBody
                };

                return mess;
            }
            catch (Exception ex)
            {
                _log.LogError($"{GetType().Name}. Error: {ex.Message}");

                throw ex;
            }
        }

        protected override string GetSubmitterReceipt(string archiveReference)
        {
            try
            {
                string adresse = base.FormData.eiendomByggested.First().adresse.adresselinje1;
                string planNavn = base.FormData.planforslag.plannavn == null ? "" : base.FormData.planforslag.plannavn;
                string byggested = adresse != null && adresse.Trim().Length > 0 ? $"{adresse}, {planNavn}" : $"{planNavn}";
                string kontaktperson = FormData.forslagsstiller.kontaktperson.navn; // GetContactPerson();
                string forslagsstiller = FormData.forslagsstiller.navn;

                //Get html embedded resource file
                string htmlBody = "";
                //var HtmlBodyTemplate = "VarselOppstartPlanarbeidReceiptMessageBody.html";
                var HtmlBodyTemplate = "FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Report.VarselOppstartPlanarbeidReceipt.html";

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name.ToUpper().Contains("FTB_FORMLOGIC"))
                    {
                        _log.LogDebug($"{GetType().Name}. Found assembly: {assembly.FullName}");
                        using (Stream stream = assembly.GetManifestResourceStream(HtmlBodyTemplate))
                        {
                            if (stream == null)
                            {
                                throw new Exception($"The resource {HtmlBodyTemplate} was not loaded properly.");
                            }

                            using (StreamReader reader = new StreamReader(stream))
                            {
                                htmlBody = reader.ReadToEnd();
                            }
                        }
                    }
                }
                //_log.LogDebug($"{GetType().Name}: htmlBody: {htmlBody}");
                //string LogoResourceName = "KommIT.FIKS.AdapterAltinnSvarUt.Content.images.dibk_logo.png";
                //TODO: Add Logo?
                //TODO: 
                //planNavn
                //forslagsstiller
                //arkivReferanse
                //vedlegg
                //naboer

                htmlBody = htmlBody.Replace("<planNavn/>", planNavn);
                htmlBody = htmlBody.Replace("<forslagsstiller/>", forslagsstiller);
                htmlBody = htmlBody.Replace("<byggested/>", byggested);
                htmlBody = htmlBody.Replace("<kontaktperson/>", kontaktperson);
                htmlBody = htmlBody.Replace("<arkivReferanse/>", archiveReference.ToUpper());
                //What attachments to add?
                var blobStorageTypes = new List<BlobStorageMetadataTypeEnum>();
                blobStorageTypes.Add(BlobStorageMetadataTypeEnum.MainForm);
                blobStorageTypes.Add(BlobStorageMetadataTypeEnum.SubmittalAttachment);

                IEnumerable<Tuple<string, string>> listOfAttachmentsInSubmittal = _blobOperations.GetListOfBlobsWithMetadataType(archiveReference, blobStorageTypes);
                string htmlText = AddTableOfAttachmentsToHtml(listOfAttachmentsInSubmittal, "Følgende vedlegg er sendt med varselet:");
                htmlBody = htmlBody.Replace("<vedlegg/>", htmlText);

                return htmlBody;
            }
            catch (Exception ex)
            {
                _log.LogError($"{GetType().Name}. Error: {ex.Message}");

                throw ex;
            }
        }
        //public override string Execute(ReportQueueItem reportQueueItem)
        //{
        //    var returnItem =  base.Execute(reportQueueItem);


        //    return returnItem;
        //}

    }
}
