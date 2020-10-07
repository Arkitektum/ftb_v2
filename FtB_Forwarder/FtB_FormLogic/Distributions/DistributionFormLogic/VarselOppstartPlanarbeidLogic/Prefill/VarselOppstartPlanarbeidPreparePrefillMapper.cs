﻿using FtB_Common.Utils;
using FtB_DataModels.Mappers;
using System.Linq;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidPreparePrefillMapper : IFormMapper<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType, no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType>
    {
        public string FormDataString { get; set; }

        public no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType Map(no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType form, string receiverId)
        {
            //var berortPart = form.beroerteParter?.Where(b => b.foedselsnummer.Equals(filter) || b.organisasjonsnummer.Equals(filter)).ToList();

            var berortPart = form.beroerteParter?.Where(b => (b.foedselsnummer != null && b.foedselsnummer.Equals(receiverId)) || (b.organisasjonsnummer != null && b.organisasjonsnummer.Equals(receiverId))).ToList();

            var svarPaaNabovarsel = new no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType();

            svarPaaNabovarsel.forslagsstiller = NabovarselPlanMappers.GetNabovarselForslagsstillerTypeMapper()
               .Map<no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType, no.kxml.skjema.dibk.nabovarselsvarPlan.ForslagsstillerType>(form.forslagsstiller);

            svarPaaNabovarsel.forslagsstiller.epost = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.epost : form.forslagsstiller.epost;
            svarPaaNabovarsel.forslagsstiller.telefon = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.telefonnummer : form.forslagsstiller.telefon;

            svarPaaNabovarsel.beroertPart = NabovarselPlanMappers.GetNabovarselBerortPartMapper()
                .Map<no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType, no.kxml.skjema.dibk.nabovarselsvarPlan.BeroertPartType>(berortPart.First());

            svarPaaNabovarsel.hovedinnsendingsnummer = form.metadata.hovedinnsendingsnummer;
            svarPaaNabovarsel.fraSluttbrukersystem = form.metadata.fraSluttbrukersystem;
            svarPaaNabovarsel.planNavn = form.planforslag.plannavn;
            svarPaaNabovarsel.planid = form.planforslag.arealplanId;
            svarPaaNabovarsel.kommune = form.kommunenavn;
            svarPaaNabovarsel.fristForInnspill = form.planforslag.fristForInnspill;
            svarPaaNabovarsel.fristForInnspillSpecified = form.planforslag.fristForInnspillSpecified;

            FormDataString = SerializeUtil.Serialize(svarPaaNabovarsel);

            return svarPaaNabovarsel;
        }
    }
}