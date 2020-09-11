using FtB_CommonModel.BusinessModels;
using System;

namespace FtB_CommonModel.Forms
{
    public abstract class FormBase
    {
        public string Name { get; protected set; }
        public string ReceiverIdentifer { get; protected set; }
        public ReceiverType ReceiverType { get; protected set; }
        public string DataFormatId { get; protected set; }
        public string DataFormatVersion { get; protected set; }
        public string SchemaFile { get; protected set; }

        public abstract void InitiateForm(string formDataAsXml);

        public abstract void ProcessPrepareStep();
        public abstract void ProcessSendStep();





        public virtual void OptionalMethod()
        {
            Console.WriteLine("Felles valgfri metode som kan kjøres for skjemaer");
        }
    }
}