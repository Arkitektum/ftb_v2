using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using FtB_DistributionForwarding.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_InitiateForwarding
{
    public class Forwarder
    {
        private PrepareStrategyBase _prepareForwarding;
        private SendStrategyBase _exceuteForwarding;
        public Forwarder(AbstractChannelFactory channel, FormBase form)
        {
            _prepareForwarding = channel.CreatePrepareBase(form);
            _exceuteForwarding = channel.CreateSendBase(form);
        }

        public void PrepareFormForForwarding()
        {
            _prepareForwarding.CommonFunc();
            
            _prepareForwarding.ReadFromSubmittalQueue("");
            _prepareForwarding.ReadReceiverInformation("");
            _prepareForwarding.CreateSubmittalDatabaseStatus("");

            _prepareForwarding.ProcessForm();
            _prepareForwarding.Exceute();
        }
        public void ExecuteForwarding()
        {
            _exceuteForwarding.ForwardToReceiver();
            _exceuteForwarding.GetFormsAndAttachmentsFromBlobStorage();
        }
    }
}
