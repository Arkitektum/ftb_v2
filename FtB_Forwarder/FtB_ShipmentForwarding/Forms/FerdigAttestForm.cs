using FtB_CommonModel.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_ShipmentForwarding.Forms
{
    public class FerdigAttestForm : Form
    {
        public override string GetMunicipalityCode()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        public override string GetSchemaFile()
        {
            throw new NotImplementedException();
        }

        public override void Process()
        {
            Console.WriteLine("Spesialhåndtering av skjema for FERDIGATTEST");
        }
    }
}
