using AltinnWebServices;
using AltinnWebServices.Models;
using Ftb_FormDownloader;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftb2_IntegrationTest.Mock
{
    public class AltinnDownloaderServiceMock : IAltinnDownloaderService
    {
        List<IArchivedItemMetadata> _list = new List<IArchivedItemMetadata>();
        public AltinnDownloaderServiceMock()
        {
            var item = new ArchivedItemMetadata(archiveReference: $"AR{new Random().Next(1000, 4999)}"
                                                , archivedDate: DateTime.Now
                                                , reporteeId: ""
                                                , reporteeType: "Person"
                                                , serviceCode: "2222"
                                                , serviceEditionCode: 1);
            _list.Add(item);
        }
        public List<IArchivedItemMetadata> DownloadMetadata()
        {
            return _list;
        }
    }
}
