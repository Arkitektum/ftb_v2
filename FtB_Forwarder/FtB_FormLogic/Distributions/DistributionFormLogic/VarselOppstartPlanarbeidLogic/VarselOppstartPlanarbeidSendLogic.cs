using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_DbModels;
using Ftb_Repositories;
using Microsoft.Extensions.Logging;
using no.kxml.skjema.dibk.nabovarselPlan;
using no.kxml.skjema.dibk.nabovarselsvarPlan;
using System;
using System.Collections.Generic;
using System.Linq;using System.Text;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Send)]
    public class VarselOppstartPlanarbeidSendLogic : DistributionSendLogic<NabovarselPlanType>
    {
        private readonly IBlobOperations _blobOperations;
        private readonly IDistributionDataMapper<NabovarselPlanType> _distributionDataMapper;
        private readonly VarselOppstartPlanarbeidPrefillMapper _prefillMapper;

        public VarselOppstartPlanarbeidSendLogic(IFormDataRepo repo,
                                                 ITableStorage tableStorage,
                                                 IBlobOperations blobOperations,
                                                 ILogger<VarselOppstartPlanarbeidSendLogic> log,
                                                 IDistributionAdapter distributionAdapter,
                                                 IDistributionDataMapper<NabovarselPlanType> distributionDataMapper,
                                                 VarselOppstartPlanarbeidPrefillMapper prefillMapper, DbUnitOfWork dbUnitOfWork
            )
            : base(repo, tableStorage, log, distributionAdapter, dbUnitOfWork)
        {
            _blobOperations = blobOperations;
            _distributionDataMapper = distributionDataMapper;
            _prefillMapper = prefillMapper;
        }

        //protected override void AddAttachmentsToDistribution(SendQueueItem sendQueueItem)
        //{
        //    var metadataList = new List<KeyValuePair<string, string>>();
        //    metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.MainForm)));
        //    metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.SubmittalAttachment)));
        //    var attachments = _blobOperations.GetAttachmentsByMetadata(BlobStorageEnum.Public, ArchiveReference, metadataList);
        //    if (attachments != null)
        //    {
        //        //TODO support attachment larger than 30MB
        //        //Sortering på vedlegg etter gruppe i blankett
        //        var sortedAttachments = new AttachmentSorter().GenerateSortedListOfAttachments(attachments.ToList());
        //        //DistributionMessage.NotificationMessage.Attachments = new AttachmentSorter().GenerateSortedListOfAttachments(attachments.ToList());
        //        DistributionMessage.NotificationMessage.Attachments = sortedAttachments;
        //    }
        //}
        protected override void MapPrefillData(string receiverId)
        {
            base.prefillSendData = _prefillMapper.Map(base.FormData, receiverId);

        }

        protected override void MapDistributionMessage()
        {
            base.DistributionMessage = _distributionDataMapper.GetDistributionMessage(prefillSendData, base.FormData, Guid.NewGuid(), base.ArchiveReference);
            
            //Add list of URL attachments to body
            var metadataList = new List<KeyValuePair<string, string>>();            metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.MainForm)));            metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.SubmittalAttachment)));            string publicBlobContainer = _blobOperations.GetPublicBlobContainerName(base.ArchiveReference);
            var urlToPublicAttachments = _blobOperations.GetBlobUrlsFromPublicStorageByMetadata(publicBlobContainer, metadataList);
            StringBuilder urlListAsHtml = new StringBuilder();
            foreach (var attachmentInfo in urlToPublicAttachments)
            {
                urlListAsHtml.Append($"<li><a href='{attachmentInfo.attachmentFileUrl}' target='_blank'>{attachmentInfo.attachmentFileName}</a></li>");
            }

            base.DistributionMessage.NotificationMessage.MessageData.MessageBody = base.DistributionMessage.NotificationMessage.MessageData.MessageBody.Replace("<vedleggsliste />", urlListAsHtml.ToString());
            base.MapDistributionMessage();
        }
    }
}
