using System;
using System.Collections.Generic;
using System.Text;
using Ftb_QueueRepository;
using AltinnWebServices.Models;

namespace Ftb_FormDownloader.Mapper
{
    public class AltinnMetadataMapper
    {
        public IMetadataQueueItem MapFrom(IArchivedItemMetadata altinnMetadata)
        {
            return new MetadataQueueItem()
            {
                Reference = altinnMetadata.ArchiveReference,
                ServiceCode = altinnMetadata.ServiceCode
            };
        }

        //public IEnumerable<IMetadataQueueItem> MapFrom(IEnumerable<IArchivedItemMetadata> altinnQueuedForm)
        //{
        //    var retVal = new List<IMetadataQueueItem>();
        //    foreach (var item in altinnQueuedForm)
        //    {
        //        retVal.Add(MapFrom(item));
        //    }

        //    return retVal;
        //}
    }
}
