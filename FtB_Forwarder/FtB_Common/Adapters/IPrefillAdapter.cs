using FtB_Common.Interfaces;

namespace FtB_Common.Adapters
{
    public interface IPrefillAdapter
    {
        PrefillResult SendPrefill(PrefillData prefillData);
    }
}