﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Koden er generert av et verktøy.
//     Kjøretidsversjon:4.0.30319.42000
//
//     Endringer i denne filen kan føre til feil virkemåte, og vil gå tapt hvis
//     koden genereres på nytt.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 
namespace no.kxml.skjema.dibk.nabovarselsvarPlan {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("Kode", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class KodeType {
        
        private string kodeverdiField;
        
        private string kodebeskrivelseField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string kodeverdi {
            get {
                return this.kodeverdiField;
            }
            set {
                this.kodeverdiField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string kodebeskrivelse {
            get {
                return this.kodebeskrivelseField;
            }
            set {
                this.kodebeskrivelseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("BeroertPart", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class BeroertPartType {
        
        private KodeType partstypeField;
        
        private string foedselsnummerField;
        
        private string organisasjonsnummerField;
        
        private string navnField;
        
        private string telefonField;
        
        private string epostField;
        
        private EnkelAdresseType adresseField;
        
        private string kommentarField;
        
        private GjelderEiendomType[] gjelderEiendomField;
        
        private string systemReferanseField;
        
        private KontaktpersonType kontaktpersonField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public KodeType partstype {
            get {
                return this.partstypeField;
            }
            set {
                this.partstypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string foedselsnummer {
            get {
                return this.foedselsnummerField;
            }
            set {
                this.foedselsnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string organisasjonsnummer {
            get {
                return this.organisasjonsnummerField;
            }
            set {
                this.organisasjonsnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string navn {
            get {
                return this.navnField;
            }
            set {
                this.navnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string telefon {
            get {
                return this.telefonField;
            }
            set {
                this.telefonField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string epost {
            get {
                return this.epostField;
            }
            set {
                this.epostField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public EnkelAdresseType adresse {
            get {
                return this.adresseField;
            }
            set {
                this.adresseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string kommentar {
            get {
                return this.kommentarField;
            }
            set {
                this.kommentarField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("gjeldereiendom", IsNullable=false)]
        public GjelderEiendomType[] gjelderEiendom {
            get {
                return this.gjelderEiendomField;
            }
            set {
                this.gjelderEiendomField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string systemReferanse {
            get {
                return this.systemReferanseField;
            }
            set {
                this.systemReferanseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public KontaktpersonType kontaktperson {
            get {
                return this.kontaktpersonField;
            }
            set {
                this.kontaktpersonField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("EnkelAdresse", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class EnkelAdresseType {
        
        private string adresselinje1Field;
        
        private string adresselinje2Field;
        
        private string adresselinje3Field;
        
        private string postnrField;
        
        private string poststedField;
        
        private string landkodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string adresselinje1 {
            get {
                return this.adresselinje1Field;
            }
            set {
                this.adresselinje1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string adresselinje2 {
            get {
                return this.adresselinje2Field;
            }
            set {
                this.adresselinje2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string adresselinje3 {
            get {
                return this.adresselinje3Field;
            }
            set {
                this.adresselinje3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string postnr {
            get {
                return this.postnrField;
            }
            set {
                this.postnrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string poststed {
            get {
                return this.poststedField;
            }
            set {
                this.poststedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string landkode {
            get {
                return this.landkodeField;
            }
            set {
                this.landkodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("GjelderEiendom", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class GjelderEiendomType {
        
        private string bolignummerField;
        
        private string bygningsnummerField;
        
        private MatrikkelnummerType eiendomsidentifikasjonField;
        
        private EiendommensAdresseType adresseField;
        
        private string kommunenavnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string bolignummer {
            get {
                return this.bolignummerField;
            }
            set {
                this.bolignummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string bygningsnummer {
            get {
                return this.bygningsnummerField;
            }
            set {
                this.bygningsnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MatrikkelnummerType eiendomsidentifikasjon {
            get {
                return this.eiendomsidentifikasjonField;
            }
            set {
                this.eiendomsidentifikasjonField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public EiendommensAdresseType adresse {
            get {
                return this.adresseField;
            }
            set {
                this.adresseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string kommunenavn {
            get {
                return this.kommunenavnField;
            }
            set {
                this.kommunenavnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("Matrikkelnummer", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class MatrikkelnummerType {
        
        private string kommunenummerField;
        
        private string gaardsnummerField;
        
        private string bruksnummerField;
        
        private string festenummerField;
        
        private string seksjonsnummerField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string kommunenummer {
            get {
                return this.kommunenummerField;
            }
            set {
                this.kommunenummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", IsNullable=true)]
        public string gaardsnummer {
            get {
                return this.gaardsnummerField;
            }
            set {
                this.gaardsnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", IsNullable=true)]
        public string bruksnummer {
            get {
                return this.bruksnummerField;
            }
            set {
                this.bruksnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", IsNullable=true)]
        public string festenummer {
            get {
                return this.festenummerField;
            }
            set {
                this.festenummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="integer", IsNullable=true)]
        public string seksjonsnummer {
            get {
                return this.seksjonsnummerField;
            }
            set {
                this.seksjonsnummerField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("EiendommensAdresse", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class EiendommensAdresseType {
        
        private string adresselinje1Field;
        
        private string adresselinje2Field;
        
        private string adresselinje3Field;
        
        private string postnrField;
        
        private string poststedField;
        
        private string landkodeField;
        
        private string gatenavnField;
        
        private string husnrField;
        
        private string bokstavField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string adresselinje1 {
            get {
                return this.adresselinje1Field;
            }
            set {
                this.adresselinje1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string adresselinje2 {
            get {
                return this.adresselinje2Field;
            }
            set {
                this.adresselinje2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string adresselinje3 {
            get {
                return this.adresselinje3Field;
            }
            set {
                this.adresselinje3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string postnr {
            get {
                return this.postnrField;
            }
            set {
                this.postnrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string poststed {
            get {
                return this.poststedField;
            }
            set {
                this.poststedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string landkode {
            get {
                return this.landkodeField;
            }
            set {
                this.landkodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string gatenavn {
            get {
                return this.gatenavnField;
            }
            set {
                this.gatenavnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string husnr {
            get {
                return this.husnrField;
            }
            set {
                this.husnrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string bokstav {
            get {
                return this.bokstavField;
            }
            set {
                this.bokstavField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("Kontaktperson", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class KontaktpersonType {
        
        private string navnField;
        
        private string telefonnummerField;
        
        private string mobilnummerField;
        
        private string epostField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string navn {
            get {
                return this.navnField;
            }
            set {
                this.navnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string telefonnummer {
            get {
                return this.telefonnummerField;
            }
            set {
                this.telefonnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string mobilnummer {
            get {
                return this.mobilnummerField;
            }
            set {
                this.mobilnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string epost {
            get {
                return this.epostField;
            }
            set {
                this.epostField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("Forslagsstiller", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class ForslagsstillerType {
        
        private KodeType partstypeField;
        
        private string foedselsnummerField;
        
        private string organisasjonsnummerField;
        
        private string navnField;
        
        private string telefonField;
        
        private string epostField;
        
        private EnkelAdresseType adresseField;
        
        private KontaktpersonType kontaktpersonField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public KodeType partstype {
            get {
                return this.partstypeField;
            }
            set {
                this.partstypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string foedselsnummer {
            get {
                return this.foedselsnummerField;
            }
            set {
                this.foedselsnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string organisasjonsnummer {
            get {
                return this.organisasjonsnummerField;
            }
            set {
                this.organisasjonsnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string navn {
            get {
                return this.navnField;
            }
            set {
                this.navnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string telefon {
            get {
                return this.telefonField;
            }
            set {
                this.telefonField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string epost {
            get {
                return this.epostField;
            }
            set {
                this.epostField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public EnkelAdresseType adresse {
            get {
                return this.adresseField;
            }
            set {
                this.adresseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public KontaktpersonType kontaktperson {
            get {
                return this.kontaktpersonField;
            }
            set {
                this.kontaktpersonField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("Signatur", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class SignaturType {
        
        private System.Nullable<System.DateTime> signaturdatoField;
        
        private bool signaturdatoFieldSpecified;
        
        private string signertAvField;
        
        private string signertPaaVegneAvField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> signaturdato {
            get {
                return this.signaturdatoField;
            }
            set {
                this.signaturdatoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool signaturdatoSpecified {
            get {
                return this.signaturdatoFieldSpecified;
            }
            set {
                this.signaturdatoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string signertAv {
            get {
                return this.signertAvField;
            }
            set {
                this.signertAvField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string signertPaaVegneAv {
            get {
                return this.signertPaaVegneAvField;
            }
            set {
                this.signertPaaVegneAvField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1")]
    [System.Xml.Serialization.XmlRootAttribute("SvarPaaNabovarselPlan", Namespace="http://skjema.kxml.no/dibk/svarPaNabovarselPlan/0.1", IsNullable=false)]
    public partial class SvarPaaNabovarselPlanType {
        
        private string hovedinnsendingsnummerField;
        
        private BeroertPartType beroertPartField;
        
        private string fraSluttbrukersystemField;
        
        private ForslagsstillerType forslagsstillerField;
        
        private string saksnummerField;
        
        private SignaturType signaturField;
        
        private string planidField;
        
        private string planNavnField;
        
        private System.Nullable<System.DateTime> fristForInnspillField;
        
        private bool fristForInnspillFieldSpecified;
        
        private string kommuneField;
        
        private string dataFormatProviderField;
        
        private string dataFormatIdField;
        
        private string dataFormatVersionField;
        
        public SvarPaaNabovarselPlanType() {
            this.dataFormatProviderField = "SERES";
            this.dataFormatIdField = "6326";
            this.dataFormatVersionField = "44843";
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string hovedinnsendingsnummer {
            get {
                return this.hovedinnsendingsnummerField;
            }
            set {
                this.hovedinnsendingsnummerField = value;
            }
        }
        
        /// <remarks/>
        public BeroertPartType beroertPart {
            get {
                return this.beroertPartField;
            }
            set {
                this.beroertPartField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string fraSluttbrukersystem {
            get {
                return this.fraSluttbrukersystemField;
            }
            set {
                this.fraSluttbrukersystemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public ForslagsstillerType forslagsstiller {
            get {
                return this.forslagsstillerField;
            }
            set {
                this.forslagsstillerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string saksnummer {
            get {
                return this.saksnummerField;
            }
            set {
                this.saksnummerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public SignaturType signatur {
            get {
                return this.signaturField;
            }
            set {
                this.signaturField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string planid {
            get {
                return this.planidField;
            }
            set {
                this.planidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string planNavn {
            get {
                return this.planNavnField;
            }
            set {
                this.planNavnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> fristForInnspill {
            get {
                return this.fristForInnspillField;
            }
            set {
                this.fristForInnspillField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fristForInnspillSpecified {
            get {
                return this.fristForInnspillFieldSpecified;
            }
            set {
                this.fristForInnspillFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string kommune {
            get {
                return this.kommuneField;
            }
            set {
                this.kommuneField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataFormatProvider {
            get {
                return this.dataFormatProviderField;
            }
            set {
                this.dataFormatProviderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataFormatId {
            get {
                return this.dataFormatIdField;
            }
            set {
                this.dataFormatIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataFormatVersion {
            get {
                return this.dataFormatVersionField;
            }
            set {
                this.dataFormatVersionField = value;
            }
        }
    }
}
