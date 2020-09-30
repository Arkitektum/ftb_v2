using FtB_Common.Interfaces;

namespace FtB_Common.Adapters
{
    public interface IPrefillAdapter
    {
        void SendPrefill(PrefillData prefillData);
    }
}