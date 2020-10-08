using System;

namespace Altinn2.Adapters.WS.ServiceOwnerArchiveExternalStreamed
{
    public interface IServiceOwnerArchiveExternalStreamedClient : IDisposable
    {
        byte[] GetAttachmentData(int attachmentId);
    }
}
