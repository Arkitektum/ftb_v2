using AltinnWebServices.WS.Correspondence;

namespace Altinn2.Adapters.WS.Correspondence
{
    public interface ICorrespondenceClient
    {
        ReceiptExternal SendCorrespondence(InsertCorrespondenceV2 correspondenceItem);
    }
}