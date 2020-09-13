using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_NotificationForwarding.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_NotificationForwarding.Mappers
{
    public static class FormatIdToNotificationFormMapper
    {
        public static IForm GetForm(string formatID)
        {
            switch (formatID)
            {
                case "6173":
                    return new SvarPaaNabovarselForm();
                case "12345":
                    //return new AnnetSkjemaAvTypeNotificationForm();
                    return null;
                default:
                    return null;
            }
        }
    }
}
