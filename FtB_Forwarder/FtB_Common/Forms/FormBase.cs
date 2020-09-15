using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;

namespace FtB_Common.Forms
{
    //public abstract class FormBase<T> : IFormDataRepo<T>
    public abstract class FormBase : IFormDataRepo
    {
        //public FormBase(IFormDataRepo<T> formDataRepo)
        public FormBase()
        {

        }
        public string Name { get; protected set; }
        public string ReceiverIdentifer { get; protected set; }
        public ReceiverType ReceiverType { get; protected set; }
        public string DataFormatId { get; protected set; }
        public string DataFormatVersion { get; protected set; }
        public string SchemaFile { get; protected set; }

        public abstract void InitiateForm(string formDataAsXml);

        public abstract IStrategy GetCustomizedPrepareStrategy();
        public abstract IStrategy GetCustomizedSendStrategy();
        //public abstract void ProcessPrepareStep();
        //public abstract void ProcessSendStep();
        //public abstract void ProcessReportStep();

        public virtual void OptionalMethod()
        {
            Console.WriteLine("Felles valgfri metode som kan kjøres for skjemaer");
        }

        //public T GetFormData(string archiveReference)
        public void GetFormData(string archiveReference)
        {
            throw new NotImplementedException();
        }
    }
}