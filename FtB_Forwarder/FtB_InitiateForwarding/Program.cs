using FtB_CommonModel.Factories;
using FtB_DistributionForwarding;
using FtB_DistributionForwarding.Forms;
using FtB_NotificationForwarding;
using FtB_NotificationForwarding.Forms;
using FtB_ShipmentForwarding;
using FtB_ShipmentForwarding.Forms;
using System;

namespace FtB_InitiateForwarding
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Distribusjon / Notifikasjon / Litt shipping");
            Console.WriteLine("Trykk D (Distribusjon), N (Notifikasjon) eller S (Litt shipping)");
            Console.WriteLine("Eller; Escape (Esc) for å avslutte:");
            ConsoleKeyInfo cki;

            AbstractProcessStepFactory distributionChannel = new DistributionChannelFactory();
            AbstractProcessStepFactory notificationChannel = new NotificationChannelFactory();
            AbstractProcessStepFactory shipmentChannel = new ShipmentChannelFactory();
            
            Forwarder distributionForwarder = new Forwarder(distributionChannel, new NaboVarselPlanForm());
            Forwarder notificationForwarder = new Forwarder(notificationChannel, new SvarPaaNabovarselForm());
            Forwarder shipmentForwarder = new Forwarder(shipmentChannel, new FerdigAttestForm());

            do
            {
                Console.WriteLine(Environment.NewLine + "Velg: D (Distribusjon), N (Notifikasjon) eller S (Litt shipping)");
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.D)
                {
                    Console.WriteLine(" --- Distribusjon er valgt ---" + Environment.NewLine);
                    ChooseProcessStep(distributionForwarder);
                }
                else if (cki.Key == ConsoleKey.N)
                {
                    Console.WriteLine(" --- Notifikasjon er valgt ---" + Environment.NewLine);
                    ChooseProcessStep(notificationForwarder);
                }
                else if (cki.Key == ConsoleKey.S)
                {
                    Console.WriteLine(" --- Shipment er valgt ---" + Environment.NewLine);
                    ChooseProcessStep(shipmentForwarder);
                }
                else
                {
                    Console.WriteLine("Shipment er ikke implementert");
                }


            } while (cki.Key != ConsoleKey.Escape);
        }
        private static void ChooseProcessStep(Forwarder forwarder)
        {
            Console.WriteLine("Velg: P (Prepare), E (Execute) eller R (Report)");
            ConsoleKeyInfo cki = Console.ReadKey();
            if (cki.Key == ConsoleKey.P)
            {
                Console.WriteLine(" --- Prepare er valgt ---" + Environment.NewLine);
                forwarder.PrepareFormForForwarding();
            }
            else if (cki.Key == ConsoleKey.E)
            {
                Console.WriteLine(" --- Execute er valgt ---" + Environment.NewLine);
                forwarder.ExecuteForwarding();
            }
            else if (cki.Key == ConsoleKey.R)
            {
                Console.WriteLine(" Reporting er ikke implementert");
            }
            else
            {
                Console.WriteLine(" Ikke gyldig input");
            }
        }
    }
}
