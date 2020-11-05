using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using FtB_DataModels.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace FtB_FormLogic
{
    //public class VarselOppstartPlanarbeidSendAltinn3PrefillMapper : IFormMapper<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    //{
    //    private readonly IDecryptionFactory _decryptionFactory;

    //    public string FormDataString { get; set; }

    //    public VarselOppstartPlanarbeidSendAltinn3PrefillMapper(IDecryptionFactory decryptionFactory)
    //    {
    //        this._decryptionFactory = decryptionFactory;
    //    }

    //    public IEnumerable<IPrefillData> Map(no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType form, string receiverId)
    //    {
    //        var decryptor = _decryptionFactory.GetDecryptor();

    //        var decryptedReceiverId = decryptor.DecryptText(receiverId);


    //        var berortParter = form.beroerteParter?.Where(b => (b.foedselsnummer != null && decryptor.DecryptText(b.foedselsnummer).Equals(decryptedReceiverId))
    //                                                                || (b.organisasjonsnummer != null && b.organisasjonsnummer.Equals(decryptedReceiverId))).ToList();

    //        var svarPaaNabovarsels = new List<IPrefillData>();
    //        foreach (var beroertPart in berortParter)
    //        {

    //            var svarPaaNabovarsel = new FtB_DataModels.Datamodels.NabovarelPlan.SvarPaaNabovarselPlanType();

    //        svarPaaNabovarsel.forslagsstiller = NabovarselPlanAltinn3Mappers.GetNabovarselForslagsstillerTypeMapper()
    //           .Map<no.kxml.skjema.dibk.nabovarselPlan.ForslagsstillerType, FtB_DataModels.Datamodels.NabovarelPlan.ForslagsstillerType>(form.forslagsstiller);

    //        svarPaaNabovarsel.forslagsstiller.epost = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.epost : form.forslagsstiller.epost;
    //        svarPaaNabovarsel.forslagsstiller.telefon = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.telefonnummer : form.forslagsstiller.telefon;

    //        svarPaaNabovarsel.beroertPart = NabovarselPlanAltinn3Mappers.GetNabovarselBerortPartMapper()
    //            .Map<no.kxml.skjema.dibk.nabovarselPlan.BeroertPartType, FtB_DataModels.Datamodels.NabovarelPlan.BeroertPartType>(beroertPart);

    //        svarPaaNabovarsel.hovedinnsendingsnummer = form.metadata.hovedinnsendingsnummer;
    //        svarPaaNabovarsel.fraSluttbrukersystem = form.metadata.fraSluttbrukersystem;
    //        svarPaaNabovarsel.planNavn = form.planforslag.plannavn;
    //        svarPaaNabovarsel.planid = form.planforslag.arealplanId;
    //        svarPaaNabovarsel.kommune = form.kommunenavn;
    //        svarPaaNabovarsel.fristForInnspill = form.planforslag.fristForInnspill;

    //            svarPaaNabovarsels.Add(new  svarPaaNabovarsel);
    //        }

    //        //FormDataString = SerializeUtil.Serialize(svarPaaNabovarsel);

    //        return svarPaaNabovarsels;
    //    }
    //}
}