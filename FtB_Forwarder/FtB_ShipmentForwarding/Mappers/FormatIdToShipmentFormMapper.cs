using FtB_Common.Factories;
using FtB_Common.Interfaces;
using FtB_ShipmentForwarding.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Mappers
{
    public static class FormatIdToShipmentFormMapper
    {
        public static IForm GetForm(string formatID)
        {
            switch (formatID)
            {
                case "6173":
                    return new FerdigAttestForm();
                case "12345":
                    //form = new NokLittNyShipmentForm();
                    return null;
                default:
                    return null;
            }
        }
    }
}
