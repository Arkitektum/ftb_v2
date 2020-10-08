﻿using FtB_Common.BusinessModels;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Prepare)]
    public class VarselOppstartPlanarbeidPrepareLogic : DistributionPrepareLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        public VarselOppstartPlanarbeidPrepareLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger<VarselOppstartPlanarbeidPrepareLogic> log) : base(repo, tableStorage, log)
        {
        }




        private List<Receiver> receivers;
        protected override List<Receiver> Receivers
        {
            get
            {
                if (receivers == null)
                {
                    receivers = new List<Receiver>();
                    foreach (var beroertPart in this.FormData.beroerteParter)
                    {
                        Enum.TryParse(beroertPart.partstype.kodeverdi, out ReceiverType receiverType);
                        string id;
                        if (receiverType.Equals(ReceiverType.Privatperson))
                        {
                            id = beroertPart.foedselsnummer;
                        }
                        else
                        {
                            id = beroertPart.organisasjonsnummer;
                        }
                        receivers.Add(new Receiver() { Type = receiverType, Id = id });
                    }
                };

                return receivers;
            }

            set { base.Receivers = value; }
        }

        protected override void GetReceivers()
        {
            throw new System.NotImplementedException();
        }
    }
}