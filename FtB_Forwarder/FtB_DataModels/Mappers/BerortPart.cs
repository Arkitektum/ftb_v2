namespace FtB_DataModels.Mappers
{
    public class BerortPart
    {
        public string Ssn { get; set; }
        public string Orgnr { get; set; }
        public string Navn { get; set; }
        public string Telefon { get; set; }
        public string Epost { get; set; }        
        public string Kommentar { get; set; }
        public string SystemReferanse { get; set; }
                
        //private KodeType partstypeField;
        //private EnkelAdresseType adresseField;
        //private GjelderEiendomType[] gjelderEiendomField;
        //private KontaktpersonType kontaktpersonField;
    }
}
