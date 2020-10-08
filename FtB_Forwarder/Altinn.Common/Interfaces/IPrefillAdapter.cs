using Altinn.Common.Models;

namespace Altinn.Common.Interfaces
{
    public interface IPrefillAdapter
    {
        PrefillResult SendPrefill(PrefillData prefillData);
    }
}