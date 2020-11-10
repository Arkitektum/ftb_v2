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
            distributionMessage.NotificationMessage.NotificationTemplate = "DIBK-nabo-1";
            distributionMessage.NotificationMessage.SenderEmail = "noreply@noreply.no";


            //Add notifications
            var smsContent = GetSMSNotificationMessage(prefill.FormInstance.beroertPart.organisasjonsnummer,
                                                        prefill.FormInstance.beroertPart.navn,
                                                        prefill.FormInstance.kommune,
                                                        prefill.FormInstance.fristForInnspill.ToString(),
                                                        prefill.FormInstance.forslagsstiller.navn,
                                                        archiveReference);

            var emailContent = GetEmailNotificationBody(prefill.FormInstance.beroertPart.organisasjonsnummer,
                                                         prefill.FormInstance.beroertPart.navn,
                                                         prefill.FormInstance.kommune,
                                                         prefill.FormInstance.planNavn,
                                                         prefill.FormInstance.fristForInnspill.ToString(),
                                                         mainFormData.forslagsstiller);

            
            var notifications = new List<Notification>();
            notifications.Add(new Notification() { EmailContent = emailContent, SmsContent = smsContent, Receiver = prefill.FormInstance.beroertPart.epost });

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

        public static string GetSMSNotificationMessage(string orgnr, string berortPartNavn, string kommunenavn, string fristForInnspill, string forslagsstillerNavn, string archiveReference)
        {
            string evtOrgnr = string.Empty;
            if (!string.IsNullOrEmpty(orgnr)) evtOrgnr = $" (org.nr. {orgnr})";

            var notificationBuilder = new StringBuilder();
            notificationBuilder.Append($"{berortPartNavn}{evtOrgnr} har fått varsel om oppstart av arbeid med reguleringsplan i {kommunenavn} kommune. ");
            notificationBuilder.Append($"Logg inn på www.altinn.no for å se varselet.");
            notificationBuilder.Append($"Du må svare innen {(fristForInnspill)} hvis du vil uttale deg. Altinn-referanse: {archiveReference}.");
            notificationBuilder.Append($"Hilsen {forslagsstillerNavn}");
            
            return notificationBuilder.ToString();
        }

        public static string GetEmailNotificationBody(string orgnr, string nabo, string kommunenavn, string plannavn, string fristForInnspill, no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType forslagsstiller)
        {
            string evtOrgnr = "";
            if (!string.IsNullOrEmpty(orgnr)) evtOrgnr = $"(org.nr. {orgnr})";

            // Dersom berørt part er en organisasjon


            // Melding
            var notificationBuilder = new StringBuilder();

            notificationBuilder.Append($"Til {nabo} {evtOrgnr}");

            notificationBuilder.Append($"<p>");
            notificationBuilder.Append($"Vi varsler om oppstart av arbeid med reguleringsplan i {kommunenavn} kommune. <br>");
            notificationBuilder.Append($"Navnet på reguleringsplanen er: {plannavn}.");
            notificationBuilder.Append($"</p>");

            notificationBuilder.Append($"Vi sender deg dette varselet fordi du kan være berørt eller har interesser i nærheten av området vi vil regulere.");

            notificationBuilder.Append($"<p>");
            notificationBuilder.Append($"<strong>Vil du ikke uttale deg?</strong><br> ");
            notificationBuilder.Append($"Da trenger du ikke å gjøre noe som helst.");
            notificationBuilder.Append($"</p>");

            notificationBuilder.Append($"<p>");
            notificationBuilder.Append($"<strong>Du kan uttale deg om oppstarten av reguleringsplanarbeidet</strong><br>");
            notificationBuilder.Append($"<a href='https://www.altinn.no'>Logg inn på Altinn for å se varselet</a> og svar innen {fristForInnspill}.");
            notificationBuilder.Append($"</p>");

            if (!string.IsNullOrEmpty(orgnr))
            {
                notificationBuilder.Append($"<p>");
                notificationBuilder.Append($"<strong>Om tilgang i Altinn</strong><br>");
                notificationBuilder.Append($"For å se varselet om oppstart, må du representere {nabo} {evtOrgnr} i Altinn. <br>");
                notificationBuilder.Append($"Den som skal se og svare på varselet, må ha rollen «Plan- og byggesak» i organisasjonen. ");
                notificationBuilder.Append($"Se <a href='https://dibk.no/verktoy-og-veivisere/andre-fagomrader/fellestjenester-bygg/slik-gir-du-rettigheter-til-byggesak-i-altinn/'>veileder om rettigheter i Altinn</a> for mer informasjon.");
                notificationBuilder.Append($"</p>");
            }


            notificationBuilder.Append($"<p>");
            notificationBuilder.Append($"Med vennlig hilsen,<br>");
            notificationBuilder.Append($"Forslagsstiller {forslagsstiller.navn}<br>");

            var telefon = Telefon(forslagsstiller);
            notificationBuilder.Append($"Telefon: {telefon} <br>");

            var epost = Epost(forslagsstiller);
            notificationBuilder.Append($"Epost: {epost}");

            notificationBuilder.Append($"</p>");

            notificationBuilder.Append($"<p><em>Det er ikke mulig å svare på denne e-posten. Spørsmål om innholdet i varselet, må du rette til {forslagsstiller.navn}.</em></p>");

            return notificationBuilder.ToString();
        }

        private static string Epost(no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType forslagsstiller)
        {
            string epost = "";
            if (!string.IsNullOrEmpty(forslagsstiller.kontaktperson.epost))
            {
                epost = forslagsstiller.kontaktperson.epost;
            }
            else if (!string.IsNullOrEmpty(forslagsstiller.epost))
            {
                epost = forslagsstiller.epost;
            }

            return epost;
        }

        private static string Telefon(no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType forslagsstiller)
        {
            string telefon = "";
            if (!string.IsNullOrEmpty(forslagsstiller.kontaktperson.telefonnummer))
            {
                telefon = forslagsstiller.kontaktperson.telefonnummer;
            }
            else if (!string.IsNullOrEmpty(forslagsstiller.telefon))
            {
                telefon = forslagsstiller.telefon;
            }

            return telefon;
        }

    }
}
