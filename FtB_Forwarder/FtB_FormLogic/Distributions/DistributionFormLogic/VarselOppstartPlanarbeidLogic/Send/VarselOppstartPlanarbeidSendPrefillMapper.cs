using FtB_Common.Encryption;
using FtB_Common.Interfaces;
using FtB_DataModels.Mappers;
using FtB_FormLogic.Distributions.DistributionFormLogic.VarselOppstartPlanarbeidLogic.Send;
using Microsoft.Extensions.Logging;
using no.kxml.skjema.dibk.nabovarselPlan;
using System.Collections.Generic;
using System.Linq;

namespace FtB_FormLogic
{
    public class VarselOppstartPlanarbeidPrefillMapper : IFormMapper<NabovarselPlanType>
    {
        private readonly IDecryptionFactory _decryptionFactory;
        private readonly ILogger<VarselOppstartPlanarbeidPrefillMapper> _logger;

        public VarselOppstartPlanarbeidPrefillMapper(IDecryptionFactory decryptionFactory, ILogger<VarselOppstartPlanarbeidPrefillMapper> logger)
        {
            _decryptionFactory = decryptionFactory;
            _logger = logger;
        }

        public IEnumerable<IPrefillData> Map(NabovarselPlanType form, string receiverId)
        {
            //Find all "berort part" for same receiver
            _logger.LogDebug("Retreives decryptor");
            var decryptor = _decryptionFactory.GetDecryptor();

            _logger.LogDebug("Decrypts receiver");
            var decryptedReceiverId = decryptor.DecryptText(receiverId);

            _logger.LogDebug("Finds berorte parter");
            var berortParter = form.beroerteParter?.Where(b => (b.foedselsnummer != null && decryptor.DecryptText(b.foedselsnummer).Equals(decryptedReceiverId)) 
                                                                    || (b.organisasjonsnummer != null && b.organisasjonsnummer.Equals(decryptedReceiverId))).ToList();
            var svarPaaNabovarsels = new List<VarselOppstartPlanarbeidData>();
            foreach (var beroertPart in berortParter)
            {
                var svarPaaNabovarsel = new no.kxml.skjema.dibk.nabovarselsvarPlan.SvarPaaNabovarselPlanType();

                svarPaaNabovarsel.forslagsstiller = NabovarselPlanMappers.GetNabovarselForslagsstillerTypeMapper()
                   .Map<PartType, no.kxml.skjema.dibk.nabovarselsvarPlan.PartType>(form.forslagsstiller);

                svarPaaNabovarsel.forslagsstiller.epost = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.epost : form.forslagsstiller.epost;
                svarPaaNabovarsel.forslagsstiller.telefon = form.forslagsstiller.kontaktperson != null ? form.forslagsstiller.kontaktperson.telefonnummer : form.forslagsstiller.telefon;

                svarPaaNabovarsel.beroertPart = NabovarselPlanMappers.GetNabovarselBerortPartMapper()
                    .Map<BeroertPartType, no.kxml.skjema.dibk.nabovarselsvarPlan.BeroertPartType>(beroertPart);

                svarPaaNabovarsel.hovedinnsendingsnummer = form.metadata.hovedinnsendingsnummer;
                svarPaaNabovarsel.fraSluttbrukersystem = form.metadata.fraSluttbrukersystem;
                svarPaaNabovarsel.planNavn = form.planforslag.plannavn;
                svarPaaNabovarsel.planid = form.planforslag.arealplanId;
                svarPaaNabovarsel.kommune = form.kommunenavn;
                svarPaaNabovarsel.fristForInnspill = form.planforslag.fristForInnspill;
                svarPaaNabovarsel.fristForInnspillSpecified = form.planforslag.fristForInnspillSpecified;

                svarPaaNabovarsels.Add(new VarselOppstartPlanarbeidData(svarPaaNabovarsel));
            }

            _logger.LogDebug("Returns prefilled data");
            return svarPaaNabovarsels;
        }
    }
}