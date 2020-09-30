using System;

namespace AltinnWebServices.WS.Prefill
{
    public interface IAltinnPrefillClient
    {
        ReceiptExternal SendPrefill(PrefillFormTask prefillFormTask, DateTime? dueDate);
    }
}