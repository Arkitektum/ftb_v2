using AltinnWebServices.WS.Correspondence;
using System.Threading.Tasks;

namespace Altinn2.Adapters.WS.Correspondence
{
    public interface ICorrespondenceClient
    {
        Task<ReceiptExternal> SendCorrespondence(InsertCorrespondenceV2 correspondenceItem, string externalShipmentReference);
    }
}