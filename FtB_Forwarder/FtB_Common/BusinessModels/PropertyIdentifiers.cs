using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.BusinessModels
{
    public class PropertyIdentifiers
    {
        public string Kommunenummer { get; set; }
        public string Gaardsnummer { get; set; }
        public string Bruksnummer { get; set; }
        public string Festenummer { get; set; }
        public string Seksjonsnummer { get; set; }
        public string Adresselinje1 { get; set; }
        public string Adresselinje2 { get; set; }
        public string Adresselinje3 { get; set; }
        public string Postnr { get; set; }
        public string Poststed { get; set; }
        public string Landkode { get; set; }
        public string Bygningsnummer { get; set; }
        public string Bolignummer { get; set; }
        public string SoeknadSkjemaNavn { get; set; }
        //public string SoeknadSkjemaSubType { get; set; } = String.Empty;
        public string TiltakType { get; set; } = String.Empty;

        public string KommunensSaksnummerAar { get; set; }
        public string KommunensSaksnummerSekvensnummer { get; set; }

        public string AnsvarligSokerNavn { get; set; }
        public string AnsvarligSokerOrgnr { get; set; }
        public string AnsvarligSokerFnr { get; set; }
        public string AnsvarligSokerAdresselinje1 { get; set; }
        public string AnsvarligSokerPostnr { get; set; }
        public string AnsvarligSokerPoststed { get; set; }

        public string FraSluttbrukersystem { get; set; }

        // For validating reportee in forms directly submitted to ansvarlig soker
        public string AnsvarligForetakOrgNr { get; set; }
    }
}
