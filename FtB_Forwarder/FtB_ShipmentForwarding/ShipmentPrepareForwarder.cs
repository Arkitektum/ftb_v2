using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding
{
    public class ShipmentPrepareForwarder : PrepareForwarding, IPrepare
    {
        public ShipmentPrepareForwarder(Form form) : base(form)
        {

        }
        public override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for SHIPMENT");
        }

        public override void ReadReceiverInformation(string archiveReference)
        {
            Console.WriteLine("Leser mottakerinformasjon for SHIPMENT");
        }

        public void TransformSubmittalToForwardingMessage()
        {
            Console.WriteLine("Transformerer innsending til (antall) mottakere for SHIPMENT");
        }
    }
}
