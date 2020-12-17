﻿using Altinn.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public class SvarPaaVarselOmOppstartAvPlanarbeidModel
    {
        public AltinnReceiverType AltinnReceiverType { get; set; }
        public string Id { get; set; }
        public string PlanId { get; set; }
        public string PlanNavn { get; set; }
        public string InitialArchiveReference { get; set; }
        public DateTime FristForInnspill { get; set; }
        public List<SvarPaaVarselOmOppstartAvPlanarbeidSenderModel> Senders { get; set; }
        public List<AttachmentBinary> Attachments { get; set; }
    }
}