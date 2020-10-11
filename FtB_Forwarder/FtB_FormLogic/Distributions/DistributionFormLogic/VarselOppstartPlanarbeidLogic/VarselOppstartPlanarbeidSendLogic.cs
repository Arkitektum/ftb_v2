using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Send)]
    public class VarselOppstartPlanarbeidSendLogic : DistributionSendLogic<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    {
        //private VarselOppstartPlanarbeidPrepareAltinn3PrefillMapper prefillMapper;
        private VarselOppstartPlanarbeidPreparePrefillMapper _prefillMapper;

        public VarselOppstartPlanarbeidSendLogic(IFormDataRepo repo, ITableStorage tableStorage, ILogger<VarselOppstartPlanarbeidSendLogic> log, IDistributionAdapter distributionAdapter) : base(repo, tableStorage, log, distributionAdapter)
        {
            //prefillMapper = new VarselOppstartPlanarbeidPrepareAltinn3PrefillMapper();
            _prefillMapper = new VarselOppstartPlanarbeidPreparePrefillMapper();
        }

        protected override void MapPrefillData(string receiverId)
        {
            _prefillMapper.Map(base.FormData, receiverId);
            base.DistributionMessage = new VarselOppstartPlanarbeidSendDataProvider().GetDistributionMessage(_prefillMapper.FormDataString, base.FormData, Guid.NewGuid().ToString());
                                    
        }
    }

    //public interface IDistributionMessageProvider<T>
    //{
    //    Altinn.Common.Models.MessageDataType CreateDistributionMessage(T formData);
    //}

    //public class VarselOppstartPlanarbeidMessageProvider : IDistributionMessageProvider<no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType>
    //{
    //    public MessageDataType CreateDistributionMessage(no.kxml.skjema.dibk.nabovarselPlan.NabovarselPlanType formData)
    //    {
    //        var retVal = new MessageDataType();
            
    //        //Create body, title and summary
           

    //        return retVal;
    //    }
    //}
}
