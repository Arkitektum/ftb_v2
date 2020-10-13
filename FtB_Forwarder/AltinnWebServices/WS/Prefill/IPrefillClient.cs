using AltinnWebServices.WS.Prefill;
using System;

namespace Altinn2.Adapters.WS.Prefill
{
    public interface IPrefillClient
    {
        ReceiptExternal SendPrefill(PrefillFormTask prefillFormTask, DateTime? dueDate);
    }
}