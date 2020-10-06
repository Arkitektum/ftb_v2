using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_FormLogic
{
    public abstract class DistributionFormLogicBase<T, TDistr> : FormLogicBase<T>
    {
        public DistributionFormLogicBase(IFormDataRepo repo, IPrefillDataProvider<TDistr> prefillDataProvider) : base(repo)
        {
            PrefillDataProvider = prefillDataProvider;
        }

        protected virtual IFormMapper<T, TDistr> Mapper { get; set; }

        protected virtual IPrefillDataProvider<TDistr> PrefillDataProvider { get; set; }

        public override PrefillData GetPrefillData(string receiverId, string distributionFormId)
        {
            Mapper.Map(this.DataForm, receiverId);
            //this.DistributionData = Mapper.FormDataString;

            return PrefillDataProvider.GetPrefillData(Mapper.FormDataString, distributionFormId);
        }

        public override void ProcessSendStep(string filter)
        {
            Mapper.Map(this.DataForm, filter);
            this.DistributionData = Mapper.FormDataString;
            //var metaData = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("PrefillReceiver", filter) };
            //_repo.AddBytesAsBlob(base.ArchiveReference, $"Prefill-{Guid.NewGuid()}", Encoding.Default.GetBytes(this.DistributionData), metaData);

            //_prefillService.SendPrefill(base.ArchiveReference, filter);

            base.ProcessSendStep(filter);

            
        }

        public override void InitiateForm()
        { }


    }
}
