using AltinnWebServices.Models;
using System.Collections.Generic;

namespace AltinnWebServices.WS.AltinnDownloadQueue
{
    public static class AltinnDownloadQueueMapper
    {
        public static AltinnArchivedItem MapArchivedForm(ArchivedFormTaskDQBE archivedItem)
        {
            var retVal = new AltinnArchivedItem()
            {
                ArchiveReference = archivedItem.ArchiveReference,
                ArchiveTimeStamp = archivedItem.ArchiveTimeStamp,
                AttachmentCount = archivedItem.AttachmentsInResponse,
                CaseID = archivedItem.CaseID,
                CorrelationReference = archivedItem.CorrelationReference,
                FormsCount = archivedItem.FormsInResponse,
                Reportee = archivedItem.Reportee,
                ServiceCode = archivedItem.ServiceCode,
                ServiceEditionCode = archivedItem.ServiceEditionCode
            };

            foreach (var attachment in archivedItem.Attachments)
            {
                retVal.Attachments.Add(new AttachmentItem(attachment.ArchiveReference, attachment.AttachmentData, attachment.AttachmentType, attachment.FileName, attachment.IsEncrypted, attachment.AttachmentId, attachment.AttachmentTypeName, attachment.AttachmentTypeNameLanguage));
            }

            for (int i = 0; i < archivedItem.Forms.Count; i++)
            {
                var form = archivedItem.Forms[i];
                var formType = i == 0 ? AltinnFormType.MainForm : AltinnFormType.SubForm;
                retVal.Forms.Add(new FormItem(form.DataFormatID, form.DataFormatVersionID, form.FormData, form.ParentReference, form.Reference, formType));
            }

            return retVal;
        }



        public static List<IArchivedItemMetadata> MapDowloadQueueItem(List<DownloadQueueItemBE> downloadQueueItems)
        {
            var retVal = new List<IArchivedItemMetadata>();
            foreach (var item in downloadQueueItems)
            {
                retVal.Add(new ArchivedItemMetadata(item.ArchiveReference, item.ArchivedDate, item.ReporteeID, item.ReporteeType.ToString(), item.ServiceCode, item.ServiceEditionCode));
            }
            return retVal;
        }
    }
}
