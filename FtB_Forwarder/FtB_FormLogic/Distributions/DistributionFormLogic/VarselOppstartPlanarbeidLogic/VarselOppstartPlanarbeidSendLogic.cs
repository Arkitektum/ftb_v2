using Altinn.Common.Interfaces;
using Altinn.Common.Models;
using FtB_Common.BusinessModels;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_DbRepository;
using Microsoft.Extensions.Logging;
using no.kxml.skjema.dibk.nabovarselPlan;
using no.kxml.skjema.dibk.nabovarselsvarPlan;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FtB_FormLogic
{
    [FormDataFormat(DataFormatId = "6325", DataFormatVersion = "44824", ProcessingContext = FormLogicProcessingContext.Send)]
    public class VarselOppstartPlanarbeidSendLogic : DistributionSendLogic<NabovarselPlanType>
    {
        private readonly IBlobOperations _blobOperations;
        private readonly IDistributionDataMapper<SvarPaaNabovarselPlanType, NabovarselPlanType> _distributionDataMapper;
        private readonly VarselOppstartPlanarbeidPrefillMapper _prefillMapper;

        public VarselOppstartPlanarbeidSendLogic(IFormDataRepo repo,
                                                 ITableStorage tableStorage,
                                                 IBlobOperations blobOperations,
                                                 ILogger<VarselOppstartPlanarbeidSendLogic> log,
                                                 IDistributionAdapter distributionAdapter,
                                                 IDistributionDataMapper<SvarPaaNabovarselPlanType, NabovarselPlanType> distributionDataMapper,
                                                 VarselOppstartPlanarbeidPrefillMapper prefillMapper, DbUnitOfWork dbUnitOfWork
            )
            : base(repo, tableStorage, log, distributionAdapter, dbUnitOfWork)
        {
            _blobOperations = blobOperations;
            _distributionDataMapper = distributionDataMapper;
            _prefillMapper = prefillMapper;
        }

        protected override void AddAttachmentsToDistribution(SendQueueItem sendQueueItem)
        {
            var metadataList = new List<KeyValuePair<string, string>>();
            metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.MainForm)));
            metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.SubmittalAttachment)));
            var attachments = _blobOperations.GetAttachmentsByMetadata(ArchiveReference, metadataList);
            if (attachments != null)
            {
                //TODO support attachment larger than 30MB
                //Sortering på vedlegg etter gruppe i blankett
                var sortedAttachments = new AttachmentSorter().GenerateSortedListOfAttachments(attachments.ToList());
                //DistributionMessage.NotificationMessage.Attachments = new AttachmentSorter().GenerateSortedListOfAttachments(attachments.ToList());
                DistributionMessage.NotificationMessage.Attachments = sortedAttachments;
            }


        }

        protected override void MapPrefillData(string receiverId)
        {
            _prefillMapper.Map(base.FormData, receiverId);
            base.DistributionMessage = _distributionDataMapper.GetDistributionMessage(_prefillMapper.FormDataString, base.FormData, Guid.NewGuid().ToString(), base.ArchiveReference);
        }
    }
}
