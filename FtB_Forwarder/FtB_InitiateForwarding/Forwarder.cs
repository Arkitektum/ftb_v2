using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_InitiateForwarding
{
    public class Forwarder
    {
        private PrepareForwarding _prepareForwarding;
        private ExceuteForwarding _exceuteForwarding;
        public Forwarder(AbstractProcessStepFactory channel, Form form)
        {
            _prepareForwarding = channel.CreatePrepareForwarding(form);
            _exceuteForwarding = channel.CreateExceuteForwarding(form);
        }

        public void PrepareFormForForwarding()
        {
            _prepareForwarding.CommonFunc();
            
            _prepareForwarding.ReadFromSubmittalQueue("");
            _prepareForwarding.ReadReceiverInformation("");
            _prepareForwarding.CreateSubmittalDatabaseStatus("");

            _prepareForwarding.ProcessForm();
        }
        public void ExecuteForwarding()
        {
            _exceuteForwarding.ForwardToReceiver();
            _exceuteForwarding.GetFormsAndAttachmentsFromBlobStorage();
        }
    }
}
