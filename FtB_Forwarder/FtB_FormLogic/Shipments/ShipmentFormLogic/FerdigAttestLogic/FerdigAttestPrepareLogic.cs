﻿using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Ftb_Repositories;
using Ftb_Repositories.HttpClients;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Prepare)]
    public class FerdigAttestPrepareLogic : ShipmentSendLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public FerdigAttestPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger log, ISvarUtAdapter svarUtAdapter, DbUnitOfWork dbUnitOfWork, FileDownloadStatusHttpClient fileDownloadHttpClient) : 
            base(repo, tableStorage, log, svarUtAdapter, dbUnitOfWork, fileDownloadHttpClient)
        {

        }

        public string GetFormatId()
        {
            throw new NotImplementedException();
        }
    }
}
