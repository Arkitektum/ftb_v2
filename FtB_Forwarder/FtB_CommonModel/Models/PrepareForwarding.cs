using FtB_CommonModel.Forms;
using System;

namespace FtB_CommonModel.Models
{
    public abstract class PrepareForwarding
    {
        Form _formBeingProcessed;
        public PrepareForwarding(Form form)
        {
            _formBeingProcessed = form;
        }
        public abstract void ReadReceiverInformation(string archiveReference);
        public abstract void CreateSubmittalDatabaseStatus(string archiveReference);
        public void ReadFromSubmittalQueue(string archiveReference)
        {
            Console.WriteLine("Fellesmetode: Leser fra innsendingskø for både DISTRIBUTION, NOTIFICATION og SHIPMENT");
        }
        public void CommonFunc()
        {
            Console.WriteLine("Felles funksjonalitet for både DISTRIBUTION, NOTIFICATION og SHIPMENT");
        }
        public void ProcessForm()
        {
            _formBeingProcessed.Process();
        }
    }
}