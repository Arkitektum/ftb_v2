using AltinnWebServices.WS.Correspondence;
namespace Altinn2.Adapters.WS.Correspondence
{
    public static class CorrespondenceAltinnFaultExtensions
    {
        public static string ToStringExtended(this AltinnFault altinnFault)
        {
            return $"AltinnFault: {altinnFault.AltinnErrorMessage} - ErrorGuid: {altinnFault.ErrorGuid}";
        }
    }
}
