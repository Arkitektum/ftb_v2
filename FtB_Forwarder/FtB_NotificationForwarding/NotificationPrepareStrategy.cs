using FtB_CommonModel.Factories;
using FtB_CommonModel.Forms;
using FtB_CommonModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding
{
    public class NotificationPrepareStrategy : PrepareStrategyBase, IPrepare
    {
        public NotificationPrepareStrategy(FormBase form) : base(form)
        {

        }
        public override void CreateSubmittalDatabaseStatus(string archiveReference)
        {
            Console.WriteLine("Oppretter databasestatus for NOTIFICATION");
        }

        public override void ReadReceiverInformation(string archiveReference)
        {
            Console.WriteLine("Leser mottakerinformasjon for NOTIFICATION");
        }

        public void TransformSubmittalToForwardingMessage()
        {
            Console.WriteLine("Transformerer innsending til (antall) mottakere for NOTIFICATION");
        }
    }
}
