using System;

namespace FtB_CommonModel.Forms
{
    public abstract class Form
    {
        public abstract string GetSchemaFile();
        public abstract string GetName();
        public abstract string GetMunicipalityCode();

        public abstract void Process();
        public virtual void OptionalMethod()
        {
            Console.WriteLine("Felles valgfri metode som kan kjøres for skjemaer");
        }
    }
}