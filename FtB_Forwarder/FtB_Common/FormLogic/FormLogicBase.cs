using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using FtB_Common.Utils;
using System;
using System.Collections.Generic;

namespace FtB_Common.FormLogic
{
    public abstract class FormLogicBase<T> : IFormLogic
    {
        protected T DataForm;
        private readonly IFormDataRepo _repo;

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

        public abstract void InitiateForm();

        public virtual void ProcessPrepareStep()
        { }
        public virtual void ProcessSendStep()
        { }
        public virtual void ProcessReportStep()
        { }

        public virtual void OptionalMethod()
        {
            Console.WriteLine("Felles valgfri metode som kan kjøres for skjemaer");
        }

        public void LoadFormData(string archiveReference)
        {
            var data = _repo.GetFormData(archiveReference);
            DataForm = SerializeUtil.DeserializeFromString<T>(data);
        }

    }
}