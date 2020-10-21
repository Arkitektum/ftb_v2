using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_DbRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                                                   INotificationAdapter notificationAdapter,
                                                   DbUnitOfWork dbUnitOfWork)
            : base(repo, tableStorage, log, notificationAdapter, blobOperations, dbUnitOfWork)
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
        private string GetContactPersonExtendedInfo()
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

            return builder.ToString();
        }

        protected override MessageDataType GetSubmitterReceiptMessage(string archiveReference)
        {
            try
            {
                string adresse = base.FormData.eiendomByggested.First().adresse.adresselinje1;
                string planNavn = base.FormData.planforslag.plannavn == null ? "" : base.FormData.planforslag.plannavn;
                string byggested = adresse != null && adresse.Trim().Length > 0 ? $"{adresse}, {planNavn}" : $"{planNavn}";
                string kontaktperson = GetContactPersonExtendedInfo();

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
                //TODO: Add Logo?
                //string LogoResourceName = "KommIT.FIKS.AdapterAltinnSvarUt.Content.images.dibk_logo.png";
                htmlBody = htmlBody.Replace("<byggested/>", byggested);
                htmlBody = htmlBody.Replace("<kontaktperson/>", kontaktperson);
                htmlBody = htmlBody.Replace("<arkivReferanse/>", archiveReference.ToUpper());

                var mess = new MessageDataType()
                {
                    MessageTitle = $"AF-Ver.2: Kvittering - varsel for oppstart av reguleringsplanarbeid, {byggested}",
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
                //TODO: Add Logo?
                //string LogoResourceName = "KommIT.FIKS.AdapterAltinnSvarUt.Content.images.dibk_logo.png";
                htmlBody = htmlBody.Replace("<planNavn />", planNavn);
                htmlBody = htmlBody.Replace("<forslagsstiller />", forslagsstiller);
                htmlBody = htmlBody.Replace("<kontaktperson />", kontaktperson);
                htmlBody = htmlBody.Replace("<arkivReferanse />", archiveReference.ToUpper());
                //TODO: What attachments to add?
                var blobStorageTypes = new List<BlobStorageMetadataTypeEnum>();
                blobStorageTypes.Add(BlobStorageMetadataTypeEnum.MainForm);
                blobStorageTypes.Add(BlobStorageMetadataTypeEnum.SubmittalAttachment);

                IEnumerable<Tuple<string, string>> listOfAttachmentsInSubmittal = _blobOperations.GetListOfBlobsWithMetadataType(archiveReference, blobStorageTypes);
                string htmlTableOfAttachments = AddTableOfAttachmentsToHtml(listOfAttachmentsInSubmittal, "Følgende vedlegg er sendt med varselet:");
                htmlBody = htmlBody.Replace("<vedlegg />", htmlTableOfAttachments);
                htmlBody = htmlBody.Replace("<antallVarsledeMottakere />", GetReceiverSuccessfullyNotifiedCount().ToString());
                
                //TODO: Set back from calling the test method
                //var listOfDeniers = GetDigitalDisallowmentReceiverNames();
                var listOfDeniers = FOR_TEST_GetDigitalDisallowmentReceiverNames();
                var deniersASHtml = new StringBuilder();
                foreach (var denier in listOfDeniers)
                {
                    deniersASHtml.Append($"{denier}<br />");
                }
                htmlBody = htmlBody.Replace("<naboerSomIkkeKunneVarsles />", deniersASHtml.ToString());

                return htmlBody;
            }
            catch (Exception ex)
            {
                _log.LogError($"{GetType().Name}. Error: {ex.Message}");

                throw ex;
            }
        }

        private int GetReceiverSuccessfullyNotifiedCount()
        {
            SubmittalEntity submittalEntity = _tableStorage.GetTableEntity<SubmittalEntity>(ArchiveReference, ArchiveReference);
            return submittalEntity.SuccessCount;
        }
        private IEnumerable<string> GetDigitalDisallowmentReceiverNames()
        {
            var receiversProcessedFromSubmittal = _tableStorage.GetReceivers(ArchiveReference);
            var digitalDisallowmentReceiverIds = receiversProcessedFromSubmittal
                    .Where(x => x.Status == Enum.GetName(typeof(ReceiverStatusEnum), ReceiverStatusEnum.DigitalDisallowment))
                    .Select( x => x.ReceiverId);

            var socialSecurityNumbers = FormData.beroerteParter
                    .Where(x => x.foedselsnummer != null)
                    .Select(x => new { Id = x.foedselsnummer, x.navn });
            var orgNumbers = FormData.beroerteParter
                    .Where(x => x.organisasjonsnummer != null)
                    .Select(x => new { Id = x.organisasjonsnummer, x.navn });

            var denierNames = socialSecurityNumbers.Union(orgNumbers)
                    .Where(x => digitalDisallowmentReceiverIds.Any(y => y == x.Id))
                    .Select(x => x.navn);

            return denierNames;
        }

        public IEnumerable<string> FOR_TEST_GetDigitalDisallowmentReceiverNames()
        {
            var receiversProcessedFromSubmittal = _tableStorage.GetReceivers(ArchiveReference);
            var digitalDisallowmentReceiverIds = receiversProcessedFromSubmittal.First().ReceiverId;

            var socialSecurityNumbers = FormData.beroerteParter
                    .Where(x => x.foedselsnummer != null)
                    .Select(x => new { Id = x.foedselsnummer, x.navn });
            var orgNumbers = FormData.beroerteParter
                    .Where(x => x.organisasjonsnummer != null)
                    .Select(x => new { Id = x.organisasjonsnummer, x.navn });

            var denierNames = socialSecurityNumbers.Union(orgNumbers)
                    .Where(x => x.Id == digitalDisallowmentReceiverIds)
                    .Select(x => x.navn);

            return denierNames;
        }
        //public override string Execute(ReportQueueItem reportQueueItem)
        //{
        //    var returnItem =  base.Execute(reportQueueItem);


        //    return returnItem;
        //}

    }
}
