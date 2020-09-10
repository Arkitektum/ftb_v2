using FtB_CommonModel.Factories;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding
{
    public class ShipmentExecuteForwarder : ExceuteForwarding, IExecute
    {
        public override void ForwardToReceiver()
        {
            Console.WriteLine("Sender skjema til SHIPMENT");
        }

        public override void GetFormsAndAttachmentsFromBlobStorage()
        {
            Console.WriteLine("Ikke implementert ennå....");
            //Console.WriteLine("Henter skjema og vedlegg for SHIPMENT");
        }
    }
}
