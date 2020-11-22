using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FtB_Common.FormLogic
{
    public abstract class FormLogicBase<T> //: IFormLogic
    {
        protected T DataForm;
        protected readonly IFormDataRepo _repo;

        public FormLogicBase(IFormDataRepo repo)
        {
            _repo = repo;
            Receivers = new List<Receiver>();
        }
        public string Name { get; protected set; }
        public ReceiverType ReceiverType { get; set; }
        public string DataFormatId { get; set; }
        public string DataFormatVersion { get; set; }
        public string SchemaFile { get; set; }
        public string ArchiveReference { get; set; }
        public virtual List<Receiver> Receivers { get; set; }
        public virtual string DistributionData { get ; set ; }

        public abstract void InitiateForm();

        public virtual void ProcessPrepareStep()
        { }
        public virtual void ProcessSendStep(string filter)
        { }
        public virtual void ProcessReportStep()
        { }

        public async Task LoadFormData(string archiveReference)
        {
            this.ArchiveReference = archiveReference;
            var data = await _repo.GetFormData(archiveReference);
            DataForm = SerializeUtil.DeserializeFromString<T>(data);
        }
        //public abstract IFormDataValidator GetFormDataValidator();
    }
}