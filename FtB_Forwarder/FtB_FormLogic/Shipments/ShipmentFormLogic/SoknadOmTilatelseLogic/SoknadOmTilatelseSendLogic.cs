using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic.OTSFormLogic
{

    [FormDataFormat(DataFormatId = "Arbeidstilsynet", DataFormatVersion = "1111", ProcessingContext = FormLogicProcessingContext.Send)]
    public class SoknadOmTilatelseSendLogic : ShipmentSendLogic<no.kxml.skjema.dibk.arbeidstilsynetsSamtykke.ArbeidstilsynetsSamtykkeType>
    {
        public SoknadOmTilatelseSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, ISvarUtAdapter svarUtAdapter) : base(repo, tableStorage, log, svarUtAdapter)
        {
        }

        protected override Receiver Receiver { get => new Receiver() { Id = "910748548", Type = ReceiverType.Foretak }; set => base.Receiver = value; }


    }
}
