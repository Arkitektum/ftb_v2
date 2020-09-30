namespace AltinnWebServices.WS.Prefill
{
    public interface IPrefillFormTaskBuilder
    {
        void AddEmailAndSmsNotification(string fromEmail, string toEmail, string subject, string emailContent, string notificationTemplate, string smsContent = null);
        void AddPrefillForm(string dataFormatId, int dataFormatVersion, string formDataXml, string sendersReference);
        void AddPrefillFormTaskAttachment(string name, string filename, byte[] attachmentData, AttachmentType attachmentType, string sendersReference);
        void AddPreFillIdentityField(string field, string fieldvalue);
        PrefillFormTask Build();
        void SetupPrefillFormTask(string serviceCode, int serviceEdition, string reportee, string externalShipRef, string sendersReference, string receiversReference, int daysValid);
        void ValidateNotificationEndpointsWithPrefillInstantiation();
    }
}