using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Common.Models
{
    public class AttachmentSetting
    {

        public static string GetAttachmentTypeFriendlyName(string attachmentType)
        {
            string friendlyName = attachmentType;
            string find = "";
            if (Types.TryGetValue(attachmentType, out find))
            {
                friendlyName = find;
            }

            return friendlyName;
        }
        
        public static readonly Dictionary<string, string> Types = new Dictionary<string, string>()
        {
            {"Dispensasjonssoeknad", "Dispensasjonssøknad"},
            {"RedegjoerelseUnntakTEK", "Redegjørelse for unntak fra TEK"},
            {"Nabovarsel", "Nabovarsel"},
            {"KvitteringNabovarsel", "Kvittering for nabovarsel"},
            {"Nabomerknader", "Nabomerknader"},
            {"KommentarNabomerknader", "Kommentar til nabomerknader"},
            {"Situasjonsplan", "Situasjonsplan"},
            {"Avkjoerselsplan", "Avkjørselsplan"},
            {"UnderlagUtnytting", "Underlag for beregning av utnytting"},
            {"TegningEksisterendeFasade", "Tegning eksisterende fasade"},
            {"TegningEksisterendePlan", "Tegning eksisterende plan"},
            {"TegningEksisterendeSnitt", "Tegning eksisterende snitt"},
            {"TegningNyFasade", "Tegning ny fasade"},
            {"TegningNyPlan", "Tegning ny plan"},
            {"TegningNyttSnitt", "Tegning nytt snitt"},
            {"ByggesaksBIM", "ByggesaksBIM"},
            {"Situasjonskart", "Situasjonskart"},
            {"Kart", "Kart"},
            {"RedegjoerelseSkredOgFlom", "Redegjørelse skred og flomfare"},
            {"RedegjoerelseAndreNaturMiljoeforhold", "Redegjørelse andre natur og miljøforhold"},
            {"RedegjoerelseGrunnforhold", "Redegjørelse grunnforhold"},
            {"RedegjoerelseEstetikk", "Redegjørelse estetikk"},
            {"RedegjoerelseForurensetGrunn", "Redegjørelse forurenset grunn"},
            {"ErklaeringAnsvarsrett", "Erklæring om ansvarsrett"},
            {"Gjennomfoeringsplan", "Gjennomføringsplan"},
            {"BoligspesifikasjonMatrikkel", "Boligspesifikasjon i matrikkel"},
            {"UttalelseVedtakAnnenOffentligMyndighet", "Uttalelse vedtak fra annen offentlig myndighet"},
            {"UttalelseKulturminnemyndighet", "Uttalelse fra kulturminnemyndigheten"},
            {"SamtykkeArbeidstilsynet", "Samtykke fra Arbeidstilsynet"},
            {"AvkjoeringstillatelseVegmyndighet", "Avkjøringstillatelse fra vegmyndigheten"},
            {"RekvisisjonAvOppmaalingsforretning", "Rekvisisjon av oppmålingsforretning"},
            {"AvklaringVA", "Avklaring av plassering nær VA-ledninger"},
            {"AvklaringHoyspent", "Avklaring av plassering nær høyspentledning"},
            {"Annet", "Annet"},
            {"Folgebrev", "Følgebrev"},
            {"SluttrapportForAvfallsplan", "Sluttrapport for avfallsplan"},
            {"Foto", "Foto"},
            {"KvitteringInnleveringAvfall", "Kvittering for innlevering av avfall"},
            {"Organisasjonsplan", "Organisasjonsplan"},
            {"Revisjonserklaering", "Revisjonserklaering"}
        };

        public static readonly Dictionary<string, string> vedleggBlankettgruppe = new Dictionary<string, string>()
        {
            {"Dispensasjonssoeknad", "B"},
            {"RedegjoerelseUnntakTEK", "B"},
            {"Nabovarsel", "C"},
            {"Varselbrev", "C"},
            {"KvitteringNabovarsel", "C"},
            {"Nabomerknader", "C"},
            {"KommentarNabomerknader", "C"},
            {"Situasjonsplan", "D"},
            {"Avkjoerselsplan", "D"},
            {"UnderlagUtnytting", "D"},
            {"TegningEksisterendeFasade", "E"},
            {"TegningEksisterendePlan", "E"},
            {"TegningEksisterendeSnitt", "E"},
            {"TegningNyFasade", "E"},
            {"TegningNyPlan", "E"},
            {"TegningNyttSnitt", "E"},
            {"ByggesaksBIM", "E"},
            {"Folgebrev", "F"},
            {"Situasjonskart", "F"},
            {"Kart", "F"},
            {"RedegjoerelseSkredOgFlom", "F"},
            {"RedegjoerelseAndreNaturMiljoeforhold", "F"},
            {"RedegjoerelseGrunnforhold", "F"},
            {"RedegjoerelseEstetikk", "F"},
            {"RedegjoerelseForurensetGrunn", "F"},
            {"ErklaeringAnsvarsrett", "G"},
            {"Gjennomfoeringsplan", "G"},
            {"BoligspesifikasjonMatrikkel", "H"},
            {"UttalelseVedtakAnnenOffentligMyndighet", "I"},
            {"UttalelseKulturminnemyndighet", "I"},
            {"SamtykkeArbeidstilsynet", "I"},
            {"AvkjoeringstillatelseVegmyndighet", "I"},
            {"RekvisisjonAvOppmaalingsforretning", "J"},
            {"AvklaringVA", "Q"},
            {"AvklaringHoyspent", "Q"},
            {"Annet", "Q"},
            {"SluttrapportForAvfallsplan", "Q"},
            {"Foto", "Q"},
            {"KvitteringInnleveringAvfall", "Q"},
            {"Organisasjonsplan", "Q"},
            {"Revisjonserklaering", "Q"}
        };

        public static readonly Dictionary<string, string> eByggesakTypes = new Dictionary<string, string>()
        {
            {"Dispensasjonssoeknad", "SØK-DISP"},
            {"RedegjoerelseUnntakTEK", "KORR"},
            {"Nabovarsel", "KORR"},
            {"KvitteringNabovarsel", "KORR"},
            {"Nabomerknader", "KORR"},
            {"KommentarNabomerknader", "KORR"},
            {"Situasjonsplan", "KART"},
            {"Avkjoerselsplan", "ANKO"},
            {"UnderlagUtnytting", "ANKO"},
            {"TegningEksisterendeFasade", "TEGN"},
            {"TegningEksisterendePlan", "TEGN"},
            {"TegningEksisterendeSnitt", "TEGN"},
            {"TegningNyFasade", "TEGN"},
            {"TegningNyPlan", "TEGN"},
            {"TegningNyttSnitt", "TEGN"},
            {"ByggesaksBIM", "TEGN"},
            {"Situasjonskart", "KART"},
            {"Kart", "KART"},
            {"Folgebrev", "KORR"},
            {"RedegjoerelseSkredOgFlom", "KORR"},
            {"RedegjoerelseAndreNaturMiljoeforhold", "KORR"},
            {"RedegjoerelseGrunnforhold", "KORR"},
            {"RedegjoerelseEstetikk", "KORR"},
            {"RedegjoerelseForurensetGrunn", "KORR"},
            {"ErklaeringAnsvarsrett", "ANKO"},
            {"Gjennomfoeringsplan", "ANKO"},
            {"BoligspesifikasjonMatrikkel", "KORR"},
            {"UttalelseVedtakAnnenOffentligMyndighet", "KORR"},
            {"UttalelseKulturminnemyndighet", "KORR"},
            {"SamtykkeArbeidstilsynet", "KORR"},
            {"AvkjoeringstillatelseVegmyndighet", "KORR"},
            {"RekvisisjonAvOppmaalingsforretning", "KORR"},
            {"AvklaringVA", "KORR"},
            {"AvklaringHoyspent", "KORR"},
            {"Annet", "KORR"},
            {"SluttrapportForAvfallsplan", "KORR"},
            {"Foto", "FOTO"},
            {"KvitteringInnleveringAvfall", "KORR"},
            {"Organisasjonsplan", "ANKO"},
            {"Revisjonserklaering", "ANKO"}
        };

        public static readonly Dictionary<string, string> DocumentTypes = new Dictionary<string, string>()
        {
            {"SØK", "SØK    Søknad"},
            {"ANKO", "ANKO    Ansvar og kontroll"},
            {"FOTO", "FOTO    Foto"},
            {"KART", "KART    Kart"},
            {"KORR", "KORR    Korrespondanse"},
            {"TEGN", "TEGN    Tegning"},
            {"SØK-ET", "SØK-ET    Ett-trinns søknad"},
            {"SØK-RS", "SØK-RS    Søknad om rammetillatelse"},
            {"SØK-TA", "SØK-TA    Søknad om tiltak uten ansvarsrett"},
            {"SØK-DISP", "SØK-DISP    Søknad om dispensasjon"},
            {"SØK-IG", "SØK-IG    Søknad om igangsettingstillatelse"},
            {"SØK-MB", "SØK-MB    Søknad om midlertidig brukstillatelse"},
            {"SØK-FA", "SØK-FA    Søknad om ferdigattest"},
            {"SØK-ES", "SØK-ES    Søknad om endring av tillatelse"}

        };

        [Obsolete("Implement interface for iAltinnForm or new iSubform")]
        public static readonly Dictionary<string, string> subformDocumenttypes = new Dictionary<string, string>()
        {
            {"Gjennomfoeringsplan", "ANKO"},
            {"Gjennomføringsplan", "ANKO"},
            {"Sluttrapport for avfallsplan", "ANKO"},
            {"SoeknadPersonligAnsvarsrett", "ANKO"},
            {"Søknad om ansvarsrett for selvbygger", "ANKO"},
            {"Gjenpartnabovarsel", "KORR"},
            {"Opplysninger gitt i nabovarsel", "KORR"}

        }; //Avfallsplan og arbeidstilsynet

        public static readonly Dictionary<string, string> eByggesakServicecodeDoctypes = new Dictionary<string, string>()
        {
            {"4528", "SØK-ET"},
            {"4397", "SØK-RS"},
            {"4373", "SØK-TA"},
            {"4401", "SØK-IG"},
            {"4399", "SØK-MB"},
            {"4400", "SØK-FA"},
            {"4655", "Nabovarsel"},
            {"4699", "Nabovarsel"},
            {"4762", "ErklaeringAnsvarsrett"},
            {"4419", "ErklaeringAnsvarsrett"},
            {"4965", "ErklaeringAnsvarsrett"},
            {"4592", "Samsvarserklæring"},
            {"4402", "SØK-ES"},
            {"4398", "Gjennomføringsplan"},
            {"5194", "Samsvarserklæring"},
            {"5207", "Samsvarserklæring"},
            {"5315", "KontrollSamsvarserklaering"},
            {"5444", "Kontrollerklæring med sluttrapport"},
            {"5445", "Kontrollerklæring med sluttrapport"},
            {"5418", "NabovarselPlan"},
            {"5419", "SvarNabovarselPlan"},
            {"4845", "Arbeidstilsynet"},
            {"5303", "Tiltakshavers signatur"}

        }; //{"SØK-DISP","SØK-DISP    Søknad om dispensasjon"}

    }

}
