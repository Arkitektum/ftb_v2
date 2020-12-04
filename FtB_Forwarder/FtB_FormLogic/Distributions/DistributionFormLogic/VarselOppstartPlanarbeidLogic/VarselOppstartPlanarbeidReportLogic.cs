﻿using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using FtB_Common.Utils;
using Ftb_Repositories;
using Ftb_Repositories.HttpClients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Report)]
    public class VarselOppstartPlanarbeidReportLogic : DistributionReportLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        private readonly IBlobOperations _blobOperations;
        private readonly IHtmlUtils _htmlUtils;

        public VarselOppstartPlanarbeidReportLogic(IFormDataRepo repo,
                                                   ITableStorage tableStorage,
                                                   ILogger<VarselOppstartPlanarbeidReportLogic> log,
                                                   IBlobOperations blobOperations,
                                                   INotificationAdapter notificationAdapter,
                                                   DbUnitOfWork dbUnitOfWork,
                                                   IHtmlUtils htmlUtils,
                                                   HtmlToPdfConverterHttpClient htmlToPdfConverterHttpClient)
            : base(repo, tableStorage, log, notificationAdapter, blobOperations, dbUnitOfWork, htmlUtils, htmlToPdfConverterHttpClient)
        {
            _blobOperations = blobOperations;
            _htmlUtils = htmlUtils;
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
                string htmlBody = _htmlUtils.GetHtmlFromTemplate("FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Report.VarselOppstartPlanarbeidReceiptMessageBody.html");
                
                htmlBody = htmlBody.Replace("<byggested/>", byggested);
                htmlBody = htmlBody.Replace("<kontaktperson/>", kontaktperson);
                htmlBody = htmlBody.Replace("<arkivReferanse/>", archiveReference.ToUpper());

                var mess = new MessageDataType()
                {
                    //TODO: Remove "AF-Ver.2: " and ({DateTime.Now.ToString("HH:mm:ss")}) from MessageTitle
                    MessageTitle = $"AF-Ver.2: Kvittering - varsel for oppstart av reguleringsplanarbeid, {byggested} ({DateTime.Now.ToString("HH:mm:ss")})",
                    MessageSummary = "Trykk på vedleggene under for å laste ned varselet og kvittering med liste over hvilke berørte parter som har blitt varslet",
                    MessageBody = htmlBody
                };

                return mess;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while getting submitter receipt message");

                throw;
            }
        }

        protected override async Task<string> GetSubmitterReceipt(ReportQueueItem reportQueueItem)
        {
            try
            {
                string adresse = base.FormData.eiendomByggested.First().adresse.adresselinje1;
                string planNavn = base.FormData.planforslag.plannavn == null ? "" : base.FormData.planforslag.plannavn;
                string byggested = adresse != null && adresse.Trim().Length > 0 ? $"{adresse}, {planNavn}" : $"{planNavn}";
                string kontaktperson = FormData.forslagsstiller.kontaktperson.navn;
                string forslagsstiller = FormData.forslagsstiller.navn;
                string htmlTemplate = _htmlUtils.GetHtmlFromTemplate("FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Report.VarselOppstartPlanarbeidReceipt.html");
                htmlTemplate = htmlTemplate.Replace("<planNavn />", planNavn);
                htmlTemplate = htmlTemplate.Replace("<forslagsstiller />", forslagsstiller);
                htmlTemplate = htmlTemplate.Replace("<kontaktperson />", kontaktperson);
                htmlTemplate = htmlTemplate.Replace("<arkivReferanse />", reportQueueItem.ArchiveReference.ToUpper());
                var blobStorageTypes = new List<BlobStorageMetadataTypeEnum>();
                blobStorageTypes.Add(BlobStorageMetadataTypeEnum.MainForm);
                blobStorageTypes.Add(BlobStorageMetadataTypeEnum.SubmittalAttachment);

                string publicContainerName =  _blobOperations.GetPublicBlobContainerName(reportQueueItem.ArchiveReference.ToLower());

                IEnumerable<(string attachmentType, string fileName)> listOfAttachmentsInSubmittal = await _blobOperations.GetListOfBlobsWithMetadataType(BlobStorageEnum.Public, publicContainerName, blobStorageTypes);
                string tableRowsAsHtml = "<tr><td>" + string.Join("</td></tr><tr><td>", listOfAttachmentsInSubmittal.Select(p => p.attachmentType + "</td><td>" + p.fileName)) + "</td></tr>";
                htmlTemplate = htmlTemplate.Replace("<vedlegg />", tableRowsAsHtml);
                var successfullyNotifiedCount = await GetReceiverSuccessfullyNotifiedCountAsync(reportQueueItem);
                htmlTemplate = htmlTemplate.Replace("<antallVarsledeMottakere />", successfullyNotifiedCount.ToString());

                var listOfReservedReporteeNames = await GetReservedReporteeNames();
                var deniersASHtml = new StringBuilder();
                foreach (var denier in listOfReservedReporteeNames)
                {
                    deniersASHtml.Append($"{denier}<br />");
                }
                htmlTemplate = htmlTemplate.Replace("<naboerSomIkkeKunneVarsles />", deniersASHtml.ToString());

                return htmlTemplate;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred when getting submitter receipt");

                throw;
            }
        }

        private async Task<IEnumerable<string>> GetReservedReporteeNames()
        {
            var allReceiversInSubmittal = await _tableStorage.GetTableEntitiesAsync<ReceiverEntity>(ArchiveReference);

            var reservedReporteeReceiverIds = allReceiversInSubmittal
                    .Where(x => x.ProcessOutcome == Enum.GetName(typeof(ReceiverProcessOutcomeEnum), ReceiverProcessOutcomeEnum.ReservedReportee))
                    .Select( x => x.ReceiverId).ToList();

            var socialSecurityNumbers = FormData.beroerteParter
                    .Where(x => x.foedselsnummer != null)
                    .Select(x => new { Id = x.foedselsnummer, x.navn }).ToList();
            var orgNumbers = FormData.beroerteParter
                    .Where(x => x.organisasjonsnummer != null)
                    .Select(x => new { Id = x.organisasjonsnummer, x.navn });

            var reservedReporteeNames = socialSecurityNumbers.Union(orgNumbers)
                    .Where(x => reservedReporteeReceiverIds.Any(y => y == x.Id))
                    .Select(x => x.navn).ToList();

            return reservedReporteeNames;
        }

        protected override (string Filename, string Name) GetFileNameForMainForm()
        {
            var Filename = "Varselbrev.pdf";
            var Name = "Varselbrev";
            return (Filename, Name);
        }
    }
}
