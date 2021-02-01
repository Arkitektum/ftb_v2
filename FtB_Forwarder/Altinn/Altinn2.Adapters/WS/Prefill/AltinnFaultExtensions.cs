using AltinnWebServices.WS.Prefill;

namespace Altinn2.Adapters.WS.Prefill
{
    public static class PrefillAltinnFaultExtensions
    {
        public static string ToStringExtended(this AltinnFault altinnFault)
        {
            return $"AltinnFault: {altinnFault.AltinnErrorMessage} - ErrorGuid: {altinnFault.ErrorGuid}";
        }
    }
}
