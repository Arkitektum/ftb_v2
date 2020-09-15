using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;

namespace FtB_Common.Forms
{
    public abstract class FormBase : IForm
    {
        public FormBase()
        {

        }
        public string Name { get; protected set; }
        public string ReceiverIdentifer { get; protected set; }
        public ReceiverType ReceiverType { get; protected set; }
        public string DataFormatId { get; protected set; }
        public string DataFormatVersion { get; protected set; }
        public string SchemaFile { get; protected set; }

        public abstract void InitiateForm(string archiveReference);

        public abstract IStrategy GetCustomizedPrepareStrategy();
        public abstract IStrategy GetCustomizedSendStrategy();
        public abstract IStrategy GetCustomizedReportStrategy();

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

        //public T GetFormData(string archiveReference)
        public void GetFormData(string archiveReference)
        {
            throw new NotImplementedException();
        }

        public string GetFormatId()
        {
            throw new NotImplementedException();
        }
    }
}