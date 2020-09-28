﻿using FtB_Common.BusinessModels;
using FtB_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common
{
    public abstract class StrategyBase
    {
        protected IFormLogic FormLogicBeingProcessed;
        private readonly ITableStorage _tableStorage;
        protected string ArchiveReference;
        protected List<Receiver> Receivers;
        public StrategyBase(IFormLogic formLogic, ITableStorage tableStorage)
        {
            FormLogicBeingProcessed = formLogic;
            _tableStorage = tableStorage;
            ArchiveReference = formLogic.ArchiveReference;
            Receivers = formLogic.Receivers;
        }
        protected void MultipleUpTheReceiversForTheStrategy()
        {
            var multipleListOfReceivers = new List<Receiver>();
            FormLogicBeingProcessed.Receivers.ForEach(delegate (Receiver receiver)
            {
                for (int i = 0; i < 5; i++)
                {
                    multipleListOfReceivers.Add(new Receiver() { Type = receiver.Type, Id = receiver.Id.Substring(0,9) + i.ToString() });
                }
            });
            Receivers = multipleListOfReceivers;
        }
    }
}
