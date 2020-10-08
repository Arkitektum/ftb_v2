using FtB_Common.Utils;
using FtB_DataModels.Mappers;
using System.Linq;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidPrepareAltinn3PrefillMapper : IFormMapper<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType, FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType>
    {
        public string FormDataString { get; set; }

        public FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType Map(no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType form, string filter)
        {
            //var berortPart = form.beroerteParter?.Where(b => b.foedselsnummer.Equals(filter) || b.organisasjonsnummer.Equals(filter)).ToList();

            var berortPart = form.beroerteParter?.Where(b => (b.foedselsnummer != null && b.foedselsnummer.Equals(filter)) || (b.organisasjonsnummer != null && b.organisasjonsnummer.Equals(filter))).ToList();

            var svarPaaNabovarsel = new FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType();

            svarPaaNabovarsel.forslagsstiller = NabovarselPlanAltinn3Mappers.GetNabovarselForslagsstillerTypeMapper()
               .Map<no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType, FtB_DataModels.Datamodels.NabovarelPlan.ForslagsstillerType>(form.forslagsstiller);

            svarPaaNabovarsel.forslagsstiller.epost = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.epost : form.forslagsstiller.epost;
            svarPaaNabovarsel.forslagsstiller.telefon = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.telefonnummer : form.forslagsstiller.telefon;

            svarPaaNabovarsel.beroertPart = NabovarselPlanAltinn3Mappers.GetNabovarselBerortPartMapper()
                .Map<no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType, FtB_DataModels.Datamodels.NabovarelPlan.BeroertPartType>(berortPart.First());

            svarPaaNabovarsel.hovedinnsendingsnummer = form.metadata.hovedinnsendingsnummer;
            svarPaaNabovarsel.fraSluttbrukersystem = form.metadata.fraSluttbrukersystem;
            svarPaaNabovarsel.planNavn = form.planforslag.plannavn;
            svarPaaNabovarsel.planid = form.planforslag.arealplanId;
            svarPaaNabovarsel.kommune = form.kommunenavn;
            svarPaaNabovarsel.fristForInnspill = form.planforslag.fristForInnspill;

            FormDataString = SerializeUtil.Serialize(svarPaaNabovarsel);

            return svarPaaNabovarsel;
        }
    }
}