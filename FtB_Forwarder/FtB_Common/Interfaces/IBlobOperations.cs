namespace FtB_Common.Storage
{
    public interface IBlobOperations
    {
        string GetFormatIdFromStoredBlob(string containerName);
        int GetFormatVersionIdFromStoredBlob(string containerName);
        string GetServiceCodeFromStoredBlob(string containerName);
        string GetFormdata(string archiveReference);
    }
}