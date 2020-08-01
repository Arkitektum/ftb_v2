using System;

namespace AltinnWebServices.WS.ServiceOwnerArchiveExternalStreamed
{
    public interface IServiceOwnerArchiveExternalStreamedClient : IDisposable
    {
        byte[] GetAttachmentData(int attachmentId);
    }
}
