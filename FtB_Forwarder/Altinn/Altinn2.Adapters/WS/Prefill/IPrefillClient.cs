using AltinnWebServices.WS.Prefill;
using System;
using System.Threading.Tasks;

namespace Altinn2.Adapters.WS.Prefill
{
    public interface IPrefillClient
    {
        Task<ReceiptExternal> SendPrefill(PrefillFormTask prefillFormTask, DateTime? dueDate);
    }
}