using FtB_Common.Interfaces;

namespace AltinnWebServices.Services
{
    public interface IPrefillAdapter
    {
        void SendPrefill(PrefillData prefillData);
    }
}