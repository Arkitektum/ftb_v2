using Altinn.Common.Interfaces;
using FtB_Common.Enums;
using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using FtB_Common.Storage;
using Ftb_Repositories;
using Ftb_Repositories.HttpClients;
using Microsoft.Extensions.Logging;
using no.kxml.skjema.dibk.nabovarselPlan;
using System;
using System.Collections.Generic;using System.Text;
using System.Threading.Tasks;

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
                                                 VarselOppstartPlanarbeidPrefillMapper prefillMapper, DbUnitOfWork dbUnitOfWork,
                                                 FileDownloadStatusHttpClient fileDownloadHttpClient
            )
            : base(repo, tableStorage, log, distributionAdapter, dbUnitOfWork, fileDownloadHttpClient)
        {
            _blobOperations = blobOperations;
            _distributionDataMapper = distributionDataMapper;
            _prefillMapper = prefillMapper;
        }
        protected override void MapPrefillData(string receiverId)
        {
            base.PrefillSendData = _prefillMapper.Map(base.FormData, receiverId);
        }

        protected override async Task MapDistributionMessage(Guid guid)
        {
            base.DistributionMessage = _distributionDataMapper.GetDistributionMessage(PrefillSendData, base.FormData, guid, base.ArchiveReference);

            //Add list of URL attachments to body
            var metadataList = new List<KeyValuePair<string, string>>();            metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.MainForm)));            metadataList.Add(new KeyValuePair<string, string>("Type", Enum.GetName(typeof(BlobStorageMetadataTypeEnum), BlobStorageMetadataTypeEnum.SubmittalAttachment)));            var publicBlobContainer = _blobOperations.GetPublicBlobContainerName(base.ArchiveReference);
            var urlToPublicAttachments = await _blobOperations.GetBlobUrlsFromPublicStorageByMetadataAsync(publicBlobContainer, metadataList);
            StringBuilder urlListAsHtml = new StringBuilder();
            foreach (var attachmentInfo in urlToPublicAttachments)
            {
                urlListAsHtml.Append($"<li><a href='{attachmentInfo.attachmentFileUrl}' target='_blank'>{attachmentInfo.attachmentFileName}</a></li>");
            }

            base.DistributionMessage.NotificationMessage.MessageData.MessageBody = base.DistributionMessage.NotificationMessage.MessageData.MessageBody.Replace("<vedleggsliste />", urlListAsHtml.ToString());
            await base.MapDistributionMessage(guid);
        }
    }
}
