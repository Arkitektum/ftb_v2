using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic.OTSFormLogic
{

    [FormDataFormat(DataFormatId = "Arbeidstilsynet", DataFormatVersion = "1111", ProcessingContext = FormLogicProcessingContext.Send)]
    public class SoknadOmTilatelseSendLogic : ShipmentSendLogic<no.kxml.skjema.dibk.arbeidstilsynetsSamtykke.ArbeidstilsynetsSamtykkeType>
    {
        public SoknadOmTilatelseSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ITableStorageOperations tableStorageOperations, ILogger log, ISvarUtAdapter svarUtAdapter, DbUnitOfWork dbUnitOfWork) : 
            base(repo, tableStorage, tableStorageOperations, log, svarUtAdapter, dbUnitOfWork)
        {
        }

        protected override Receiver Receiver { get => new Receiver() { Id = "910748548", Type = ReceiverType.Foretak }; set => base.Receiver = value; }


    }
}
