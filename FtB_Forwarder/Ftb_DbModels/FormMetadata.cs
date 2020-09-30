using System;
using System.ComponentModel.DataAnnotations;

namespace Ftb_DbModels
{
    public class FormMetadata
    {
        /// <summary>
        /// Arkivreferanse i altinn (MessageId i REST API)
        /// </summary>
        [Key]
        public string ArchiveReference { get; set; }
        /// <summary>
        /// Kommunenr det sendes til
        /// </summary>
        public string MunicipalityCode { get; set; }
        //public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Sluttbrukersystem som gjør innsending
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Referanse til svarUt forsendelsesid
        /// </summary>
        public string SvarUtForsendelsesId { get; set; }
        /// <summary>
        /// Dato for altinn har arkivert forsendelse (samme som når søker sender inn og signerer)
        /// </summary>
        public DateTime? ArchiveTimestamp { get; set; }

        /// <summary>
        /// Navn på skjematype - samme som står her https://fellesbygg.dibk.no/
        /// </summary>
        public string FormType { get; set; }
        /// <summary>
        /// Tittel som sendes til SvarUt som settes sammen av felter etter regler gitt pr kommune
        /// </summary>
        public string SvarUtDocumentTitle { get; set; }
        /// <summary>
        /// Tidspunkt for når SvarUt har akseptert forsendelse
        /// </summary>
        public DateTime? SvarUtShippingTimestamp { get; set; }
        /// <summary>
        /// Referanse gitt i skjemadata som er viktig for søknadssystem/søker
        /// </summary>
        public string VaarReferanse { get; set; }

        /// <summary>
        /// Kommunens saksnummer del saksår - kommer fra skjemadata eller kan settes av kommunen i egen kvitteringstjeneste
        /// </summary>
        public int MunicipalityArchiveCaseYear { get; set; }
        /// <summary>
        /// Kommunens saksnummer del sekvensnr - kommer fra skjemadata eller kan settes av kommunen i egen kvitteringstjeneste
        /// </summary>
        public long MunicipalityArchiveCaseSequence { get; set; }

        /// <summary>
        /// Kommunens referanse til forsendelsen i sin offentlige journal(postliste) - kan settes av kommunen i egen kvitteringstjeneste
        /// </summary>
        public string MunicipalityPublicArchiveCaseUrl { get; set; }


        /// <summary>
        /// Tjenestekode i altinn
        /// </summary>
        public string ServiceCode { get; set; }
        /// <summary>
        /// Utgavenr i altinn
        /// </summary>
        public int ServiceEditionCode { get; set; }

        /// <summary>
        /// Status på innsending - Avvist/feil, Ok, Ok - men advarsler
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Sendes via system - SvarUt/Altinn
        /// </summary>
        public string SenderSystem { get; set; }

        /// <summary>
        /// Mottaker av forsendelse
        /// </summary>
        public string SendTo { get; set; }

        /// <summary>
        /// Avsender av forsendelse
        /// </summary>
        public string SendFrom { get; set; }

        /// <summary>
        /// Antall valideringsfeil
        /// </summary>
        public int ValidationErrors { get; set; }

        /// <summary>
        /// Antall valideringsadvarsler
        /// </summary>
        public int ValidationWarnings { get; set; }

        /// <summary>
        /// Lenke til kvitteringsrapport for distribusjonssending
        /// </summary>
        public string DistributionRecieptLink { get; set; }

        /// <summary>
        /// Status i svarut
        /// </summary>
        public string SvarUtStatus { get; set; }

        /// <summary>
        /// Søkers navn
        /// </summary>
        public string ApplicantName { get; set; }

        /// <summary>
        /// Søkers adresse
        /// </summary>
        public string ApplicantAddress { get; set; }

        /// <summary>
        /// Første eiendom kommunenr i skjema
        /// </summary>
        public string PropertyFirstKnr { get; set; }

        /// <summary>
        /// Første eiendom gårdsnummer i skjema
        /// </summary>
        public string PropertyFirstGardsnr { get; set; }

        /// <summary>
        /// Første eiendom bruksnummer i skjema
        /// </summary>
        public string PropertyFirstBruksnr { get; set; }

        /// <summary>
        /// Første eiendom festenummer i skjema
        /// </summary>
        public string PropertyFirstFestenr { get; set; }

        /// <summary>
        /// Første eiendom seksjonsnummer i skjema
        /// </summary>
        public string PropertyFirstSeksjonsnr { get; set; }

        /// <summary>
        /// Første eiendom sin adresse
        /// </summary>
        public string PropertyFirstAddress { get; set; }

        /// <summary>
        /// Første tiltakstype
        /// </summary>
        public string FirstActionType { get; set; }


        public FormMetadata(string archiveReference)
        {
            ArchiveReference = archiveReference;
        }

        public FormMetadata()
        {
        }

        //public string GetMunicipalityName()
        //{
        //    return Municipality?.Name;
        //}
    }
}