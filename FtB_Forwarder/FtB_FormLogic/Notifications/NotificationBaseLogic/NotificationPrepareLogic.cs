﻿using FtB_Common.BusinessModels;
using FtB_Common.Encryption;
using FtB_Common.Enums;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ftb_DbModels;
using Ftb_Repositories.HttpClients;

namespace FtB_FormLogic
{
    public abstract class NotificationPrepareLogic<T> : PrepareLogic<T>
    {
        protected readonly IBlobOperations _blobOperations;

        public NotificationPrepareLogic(IFormDataRepo repo,
                                        ITableStorage tableStorage,
                                        IBlobOperations blobOperations,
                                        ILogger log,
                                        DbUnitOfWork dbUnitOfWork,
                                        IDecryptionFactory decryptionFactory,
                                        FileDownloadStatusHttpClient fileDownloadHttpClient)
            : base(repo, tableStorage, log, dbUnitOfWork, decryptionFactory, fileDownloadHttpClient)
        {
            _blobOperations = blobOperations;
        }
        public override async Task<IEnumerable<SendQueueItem>> ExecuteAsync(SubmittalQueueItem submittalQueueItem)
        {
            var returnValue = await base.ExecuteAsync(submittalQueueItem);
            var sendQueueItems = CreateNotificationSendQueueItem(submittalQueueItem);
            var queueList = new List<SendQueueItem>();
            queueList.Add(sendQueueItems);
            await UpdateDistributionForms();
            await AddRepliedFilesToFileDownloadStatus(submittalQueueItem.ArchiveReference);
            return queueList;
        }

        protected async Task<string> GetInitialArchiveReferenceAsync(string distributionId)
        {
            var distributionForm = await _dbUnitOfWork.DistributionForms.Get(distributionId.ToUpper());

            return distributionForm.InitialArchiveReference;
        }
        private SendQueueItem CreateNotificationSendQueueItem(SubmittalQueueItem submittalQueueItem)
        {
            return new SendQueueItem()
            {
                ArchiveReference = ArchiveReference,
                ReceiverSequenceNumber = "0",
                Receiver = Receivers[0],
                Sender = Sender
            };
        }

        public async Task UpdateDistributionForms()
        {
            var distributionId = GetHovedinnsendingsNummer();
            var distributionForm = await _dbUnitOfWork.DistributionForms.Get(distributionId.ToString());
            distributionForm.Signed = DateTime.Now;
            distributionForm.DistributionStatus = DistributionStatus.signed;
            distributionForm.SignedArchiveReference = ArchiveReference.ToUpper();

            await _dbUnitOfWork.DistributionForms.Update(distributionForm.InitialArchiveReference, distributionId, distributionForm);
        }

        public async Task AddRepliedFilesToFileDownloadStatus(string archiveReference)
        {
            var targetContainerName = GetHovedinnsendingsNummer();
            var sourceContainerName = archiveReference;
            /*
            Main form XML
                metadata: type = FormData
            */
            var mainFormXMLDataMetadataList = new List<KeyValuePair<string, string>>();
            mainFormXMLDataMetadataList.Add(new KeyValuePair<string, string>("type", "FormData"));
            var mainFormData = await _blobOperations.GetBlobContentAsBytesByMetadata(BlobStorageEnum.Private, sourceContainerName, mainFormXMLDataMetadataList);
            var uri = await _blobOperations.AddByteStreamToBlobStorage(BlobStorageEnum.Private, targetContainerName.ToString(), "SvarNabovarselPlan.xml", mainFormData.ByteContent, mainFormData.ContentType);

            var fds = new FileDownloadStatus()
            {
                ArchiveReference = archiveReference.ToUpper(),
                BlobLink = uri,
                FileAccessCount = 0,
                Filename = "SvarNabovarselPlan.xml",
                FileType = FileTypesForDownloadEnum.MaskinlesbarXml,
                FormName = "VarselOppstartPlanarbeid",
                Guid = targetContainerName,
                IsDeleted = false,
                MimeType = mainFormData.ContentType
            };

            await _fileDownloadHttpClient.Post(archiveReference, fds);

            /*          
            Main form PDF
                metadata: attachmenttypename = SvarNabovarselPlan
                          type = MainForm
            */
            var mainFormPdfDataMetadataList = new List<KeyValuePair<string, string>>();
            mainFormPdfDataMetadataList.Add(new KeyValuePair<string, string>("type", "MainForm"));

            var mainFormPdfData = await _blobOperations.GetBlobContentAsBytesByMetadata(BlobStorageEnum.Private, sourceContainerName, mainFormPdfDataMetadataList);
            var mainFormPdfUri = await _blobOperations.AddByteStreamToBlobStorage(BlobStorageEnum.Private, targetContainerName.ToString(), "SvarNabovarselPlan.pdf", mainFormPdfData.ByteContent, mainFormPdfData.ContentType);

            var mainFormPdfFds = new FileDownloadStatus()
            {
                ArchiveReference = archiveReference.ToUpper(),
                BlobLink = mainFormPdfUri,
                FileAccessCount = 0,
                Filename = "SvarNabovarselPlan.pdf",
                FileType = FileTypesForDownloadEnum.Nabovarsel,
                FormName = "VarselOppstartPlanarbeid",
                Guid = targetContainerName,
                IsDeleted = false,
                MimeType = mainFormPdfData.ContentType
            };

            await _fileDownloadHttpClient.Post(archiveReference, mainFormPdfFds);

            /*
            Attachments
                metadata: attachmenttypename = Annet
                          type = SubmittalAttachment            
            */
            var metadataList = new List<KeyValuePair<string, string>>();
            metadataList.Add(new KeyValuePair<string, string>("type", "SubmittalAttachment"));

            var attachments = _blobOperations.GetBlobContentsAsBytesByMetadata(BlobStorageEnum.Private, sourceContainerName, metadataList).ToList();

            foreach (var attachment in attachments)
            {                

                //This is probably not the correct order...
                var fileName = $"Vedlegg.{attachment.FileName}";

                var attachmentUri = await _blobOperations.AddByteStreamToBlobStorage(BlobStorageEnum.Private, targetContainerName.ToString(), fileName, attachment.ByteContent, attachment.ContentType);

                var attachmentFds = new FileDownloadStatus()
                {
                    ArchiveReference = archiveReference.ToUpper(),
                    BlobLink = attachmentUri,
                    FileAccessCount = 0,
                    Filename = fileName,
                    FileType = FileTypesForDownloadEnum.Nabomerknader,
                    FormName = "VarselOppstartPlanarbeid",
                    Guid = targetContainerName,
                    IsDeleted = false,
                    MimeType = attachment.ContentType
                };

                await _fileDownloadHttpClient.Post(archiveReference, attachmentFds);
            }
        }

        protected async Task CopyPDFToPublicBlobStorage(byte[] pdfDoc, string senderName, string publicContainer, string sendersArchiveReference)
        {
            char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            var validSenderFilename = new string(senderName.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());

            var metadataList = new List<KeyValuePair<string, string>>();
            metadataList.Add(new KeyValuePair<string, string>("AttachmentTypeName", "SvarNabovarselPlan"));
            metadataList.Add(new KeyValuePair<string, string>("SendersArchiveReference", sendersArchiveReference));
            await _blobOperations.AddByteStreamToBlobStorage(BlobStorageEnum.Public,
                                                                publicContainer,
                                                                $"Uttalelse_{validSenderFilename}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.pdf",
                                                                pdfDoc,
                                                                "application/pdf",
                                                                metadataList);
        }

        protected async Task<byte[]> GetPDFReplyFromPrivateBlobStorageAsync(string archiveReference)
        {
            var metadataList = new KeyValuePair<string, string>("AttachmentTypeName", "SvarNabovarselPlan");

            return await _blobOperations.GetBlobAsBytesByMetadata(BlobStorageEnum.Private, archiveReference.ToLower(), metadataList);
        }

        protected abstract Guid GetHovedinnsendingsNummer();

    }
}