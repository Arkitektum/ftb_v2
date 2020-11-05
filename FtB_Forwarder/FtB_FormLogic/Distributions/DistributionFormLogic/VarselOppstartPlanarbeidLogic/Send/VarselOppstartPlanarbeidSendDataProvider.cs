using Altinn.Common.Models;
using FtB_Common.Interfaces;
using FtB_DataModels.Mappers;
using FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidSendDataProvider : SendDataProviderBase, IDistributionDataMapper<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {       
        public AltinnDistributionMessage GetDistributionMessage(IEnumerable<IPrefillData> prefills, no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType mainFormData, Guid distributionFormId, string archiveReference)
        {
            var prefill = prefills.First() as VarselOppstartPlanarbeidData;

            var distributionMessage = new AltinnDistributionMessage()
            {
                PrefillDataFormatId = prefill.DataFormatId,
                PrefillDataFormatVersion = prefill.DataFormatVersion,
                DistributionFormReferenceId = distributionFormId,
                PrefillServiceCode = prefill.PrefillServiceCode,
                PrefillServiceEditionCode = prefill.PrefillServiceEditionCode,
                PrefilledXmlDataString = prefill.ToString(),
                DaysValid = 14,
                DueDate = null
            };

            distributionMessage.NotificationMessage.Receiver = base.GetReceiver(NabovarselPlanMappers.GetNabovarselReceiverMapper().Map<BerortPart>(prefill.FormInstance.beroertPart));
            distributionMessage.NotificationMessage.MessageData = CreateMessageData(mainFormData, prefill.FormInstance);
            distributionMessage.NotificationMessage.ArchiveReference = archiveReference;
            distributionMessage.NotificationMessage.ReplyLink = CreateReplyLink();
            return distributionMessage;
        }

        private MessageDataType CreateMessageData(no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType mainFormData, no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType prefillData)
        {
            var body = GetPrefillNotificationBody(prefillData.forslagsstiller, prefillData.beroertPart, prefillData.fristForInnspill, prefillData.kommune);
            var summary = string.Empty;
            var title = GetPrefillNotificationTitle(prefillData.planNavn, prefillData.planid);
            return new MessageDataType() { MessageBody = body, MessageSummary = summary, MessageTitle = title };
        }

        private ReplyLink CreateReplyLink()
        {
            var replyLink = new ReplyLink()
            {
                //URL settes av adapteret..
                UrlTitle = "Trykk her for å fylle ut svarskjemaet"
            };
            return replyLink;

        }


        public static string GetPrefillNotificationBody(no.kxml.skjema.dibk.nabovarselsvarPlan.ForslagsstillerType forslagsstiller, no.kxml.skjema.dibk.nabovarselsvarPlan.BeroertPartType beroertPart, DateTime? fristForInnspill, string kommune)
        {
            //if (pdf.HasValue && pdf.Value)
            //{
            //    createBodyForPDF(beroertPart, forslagsstiller, fristForInnspill);
            //}
            //else
            //{
            var message = new StringBuilder();
            string datoFristInnspill = String.Empty;
            if (fristForInnspill.HasValue)
            {
                datoFristInnspill = string.Format("{0:MM.dd.yyyy}", fristForInnspill);
            }

            message.Append($"{forslagsstiller.navn} ønsker å endre  eller bygge i {kommune} kommune.<br>");
            message.Append($"Klikk på dokumentene nederst i denne meldingen for å lese mer om planarbeidet, og for å se et kartutsnitt av hvilket område planene gjelder.</p><br>");

            message.Append($"<p><strong>Hvorfor får jeg varsel?</strong><br>");
            message.Append("Du får dette varselet fordi du kan være berørt eller har interesser i nærheten av området vi vil endre.");
            message.Append("</p>");


            message.Append($"<p><strong>Har du spørsmål om planene?</strong><br>");
            message.Append($"Ta kontakt på e-post {forslagsstiller.kontaktperson.epost} eller telefon {forslagsstiller.kontaktperson.telefonnummer}.</p>");

            message.Append($"<p><strong>Vil du uttale deg om planene?</strong><br>");
            message.Append($"Du kan bruke svarskjemaet under for å uttale deg om arbeidet med planen. Fristen for å sende uttalelse, er <strong>{datoFristInnspill}</strong></p>");

            message.Append($"<p><strong>Hva skjer med uttalelsene dine?</strong><br>");
            message.Append($"Uttalelsene dine blir sendt til {forslagsstiller.navn}. Du får ikke et eget svarbrev fra {forslagsstiller.navn}, men de skal vurdere alle uttalelser. Uttalelsene er med på å danne grunnlaget for forslaget som senere skal behandles av kommunen.</p>");

            message.Append($"<p>Neste skritt er at kommunen skal vurdere om planforslaget er godt nok for å sendes på høring og offentlig ettersyn. I denne fasen får du en ny mulighet til å uttale deg om detaljene i planforslaget.</p>");

            message.Append($"<p>Du kan lese mer om planarbeid på nettsiden <a href='https://www.planlegging.no'>www.planlegging.no</a></p>");

            message.Append($"<p><strong>Har du ingen uttalelser?</strong><br>");
            message.Append($"Da trenger du ikke gjøre noe som helst.</p>");

            return message.ToString();
        }

        public static string GetPrefillNotificationTitle(string planNavn, string planid)
        {
            return $"Oppstart av planarbeid - {planNavn} ({planid})";
        }

        /*

        public string GetPrefillNotificationBody(bool? pdf = false, string replyLink = "")
        {
            ForslagsstillerType forslagsstiller = form.forslagsstiller;
            BeroertPartType beroertPart = form.beroertPart;
            DateTime? fristForInnspill = form.fristForInnspill;
            string kommune = form.kommune;
            
            return NabovarselPlanMessages.GetPrefillNotificationBody(forslagsstiller, beroertPart, fristForInnspill, kommune, pdf, replyLink);

        }
 */
    }
}
