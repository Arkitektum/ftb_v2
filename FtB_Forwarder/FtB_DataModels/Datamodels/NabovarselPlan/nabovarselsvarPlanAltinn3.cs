using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
namespace FtB_DataModels.Datamodels.NabovarelPlan
{
    public class SvarPaaNabovarselPlanType
    {
        [XmlAttribute("dataFormatProvider")]
        public string dataFormatProvider { get; set; } = "SERES";
        [XmlAttribute("dataFormatId")]
        public string dataFormatId { get; set; } = "6326";
        [XmlAttribute("dataFormatVersion")]
        public string dataFormatVersion { get; set; } = "44843";
        [XmlElement("hovedinnsendingsnummer")]
        public string hovedinnsendingsnummer { get; set; }
        [XmlElement("beroertPart")]
        public BeroertPartType beroertPart { get; set; }
        [XmlElement("fraSluttbrukersystem")]
        public string fraSluttbrukersystem { get; set; }
        [XmlElement("forslagsstiller")]
        public ForslagsstillerType forslagsstiller { get; set; }
        [XmlElement("saksnummer")]
        public string saksnummer { get; set; }
        [XmlElement("signatur")]
        public SignaturType signatur { get; set; }
        [XmlElement("planid")]
        public string planid { get; set; }
        [XmlElement("planNavn")]
        public string planNavn { get; set; }
        [XmlElement("fristForInnspill")]
        public DateTime? fristForInnspill { get; set; }
        [XmlElement("kommune")]
        public string kommune { get; set; }
    }
    public class BeroertPartType
    {
        [XmlElement("partstype")]
        public KodeType partstype { get; set; }
        [XmlElement("foedselsnummer")]
        public string foedselsnummer { get; set; }
        [XmlElement("organisasjonsnummer")]
        public string organisasjonsnummer { get; set; }
        [XmlElement("navn")]
        public string navn { get; set; }
        [XmlElement("telefon")]
        public string telefon { get; set; }
        [XmlElement("epost")]
        public string epost { get; set; }
        [XmlElement("adresse")]
        public EnkelAdresseType adresse { get; set; }
        [XmlElement("kommentar")]
        public string kommentar { get; set; }
        [XmlElement("gjelderEiendom")]
        public GjelderEiendomListe gjelderEiendom { get; set; }
        [XmlElement("systemReferanse")]
        public string systemReferanse { get; set; }
        [XmlElement("kontaktperson")]
        public KontaktpersonType kontaktperson { get; set; }
    }
    public class KodeType
    {
        [XmlElement("kodeverdi")]
        public string kodeverdi { get; set; }
        [XmlElement("kodebeskrivelse")]
        [JsonPropertyName("kodebeskrivelse")]
        public string kodebeskrivelse { get; set; }
    }
    public class EnkelAdresseType
    {
        [XmlElement("adresselinje1")]
        public string adresselinje1 { get; set; }
        [XmlElement("adresselinje2")]
        public string adresselinje2 { get; set; }
        [XmlElement("adresselinje3")]
        public string adresselinje3 { get; set; }
        [XmlElement("postnr")]
        public string postnr { get; set; }
        [XmlElement("poststed")]
        public string poststed { get; set; }
        [XmlElement("landkode")]
        public string landkode { get; set; }
    }
    public class GjelderEiendomListe
    {
        [XmlElement("gjeldereiendom")]
        public List<GjelderEiendomType> gjeldereiendom { get; set; }
    }
    public class GjelderEiendomType
    {
        [XmlElement("bolignummer")]
        public string bolignummer { get; set; }
        [XmlElement("bygningsnummer")]
        public string bygningsnummer { get; set; }
        [XmlElement("eiendomsidentifikasjon")]
        public MatrikkelnummerType eiendomsidentifikasjon { get; set; }
        [XmlElement("adresse")]
        public EiendommensAdresseType adresse { get; set; }
        [XmlElement("kommunenavn")]
        public string kommunenavn { get; set; }
    }
    public class MatrikkelnummerType
    {
        [XmlElement("kommunenummer")]
        public string kommunenummer { get; set; }
        [Range(Int32.MinValue, Int32.MaxValue)]
        [XmlElement("gaardsnummer")]
        public decimal? gaardsnummer { get; set; }
        [Range(Int32.MinValue, Int32.MaxValue)]
        [XmlElement("bruksnummer")]
        public decimal? bruksnummer { get; set; }
        [Range(Int32.MinValue, Int32.MaxValue)]
        [XmlElement("festenummer")]
        public decimal? festenummer { get; set; }
        [Range(Int32.MinValue, Int32.MaxValue)]
        [XmlElement("seksjonsnummer")]
        public decimal? seksjonsnummer { get; set; }
    }
    public class EiendommensAdresseType
    {
        [XmlElement("adresselinje1")]
        public string adresselinje1 { get; set; }
        [XmlElement("adresselinje2")]
        public string adresselinje2 { get; set; }
        [XmlElement("adresselinje3")]
        public string adresselinje3 { get; set; }
        [XmlElement("postnr")]
        public string postnr { get; set; }
        [XmlElement("poststed")]
        public string poststed { get; set; }
        [XmlElement("landkode")]
        public string landkode { get; set; }
        [XmlElement("gatenavn")]
        public string gatenavn { get; set; }
        [XmlElement("husnr")]
        public string husnr { get; set; }
        [XmlElement("bokstav")]
        public string bokstav { get; set; }
    }
    public class KontaktpersonType
    {
        [XmlElement("navn")]
        public string navn { get; set; }
        [XmlElement("telefonnummer")]
        public string telefonnummer { get; set; }
        [XmlElement("mobilnummer")]
        public string mobilnummer { get; set; }
        [XmlElement("epost")]
        public string epost { get; set; }
    }
    public class ForslagsstillerType
    {
        [XmlElement("partstype")]
        public KodeType partstype { get; set; }
        [XmlElement("foedselsnummer")]
        public string foedselsnummer { get; set; }
        [XmlElement("organisasjonsnummer")]
        public string organisasjonsnummer { get; set; }
        [XmlElement("navn")]
        public string navn { get; set; }
        [XmlElement("telefon")]
        public string telefon { get; set; }
        [XmlElement("epost")]
        public string epost { get; set; }
        [XmlElement("adresse")]
        public EnkelAdresseType adresse { get; set; }
        [XmlElement("kontaktperson")]
        public KontaktpersonType kontaktperson { get; set; }
    }
    public class SignaturType
    {
        [XmlElement("signaturdato")]
        public DateTime? signaturdato { get; set; }
        [XmlElement("signertAv")]
        public string signertAv { get; set; }
        [XmlElement("signertPaaVegneAv")]
        public string signertPaaVegneAv { get; set; }
    }
}
