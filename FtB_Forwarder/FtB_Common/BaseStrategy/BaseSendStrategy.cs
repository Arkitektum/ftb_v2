﻿using FtB_Common.Interfaces;

namespace FtB_Common
{
    public abstract class BaseSendStrategy : IStrategy
    {
        IForm _formBeingProcessed;
        public BaseSendStrategy(IForm form)
        {
            _formBeingProcessed = form;
        }

        public abstract void Exceute();

        public abstract void ForwardToReceiver();
        public abstract void GetFormsAndAttachmentsFromBlobStorage();
        public void ProcessForm()
        {
            //_formBeingProcessed.ProcessSendStep();
        }
    }
}