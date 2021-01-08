using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Ftb_Repositories.HttpClients;
using Microsoft.Extensions.Logging;

namespace FtB_FormLogic.OTSFormLogic
{

    [FormDataFormat(DataFormatId = "Arbeidstilsynet", DataFormatVersion = "1111", ProcessingContext = FormLogicProcessingContext.Send)]
    public class SoknadOmTilatelseSendLogic : ShipmentSendLogic<no.kxml.skjema.dibk.arbeidstilsynetsSamtykke.ArbeidstilsynetsSamtykkeType>
    {
        public SoknadOmTilatelseSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, ISvarUtAdapter svarUtAdapter, DbUnitOfWork dbUnitOfWork, FileDownloadStatusHttpClient fileDownloadHttpClient) : 
            base(repo, tableStorage, log, svarUtAdapter, dbUnitOfWork, fileDownloadHttpClient)
        {
        }

        protected override Actor Receiver { get => new Actor() { Id = "910748548", Type = ActorType.Foretak }; set => base.Receiver = value; }


    }
}
