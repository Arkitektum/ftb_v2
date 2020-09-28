using FtB_Common.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IStorageEntity = FtB_Common.Interfaces.IStorageEntity;

namespace FtB_Common.BusinessModels
{
    public class SubmittalEntity : TableEntity, IStorageEntity
    {
        public SubmittalEntity()
        {
        }

        public SubmittalEntity(string archiveReference, int receiverCount, DateTime createdTimestamp)
        {
            this.PartitionKey = archiveReference;
            this.RowKey = archiveReference;
            this.ReceiverCount = receiverCount;
            this.SentCount = 0;
            this.CreatedTimeStamp = createdTimestamp;
        }
        public int ReceiverCount { get; set; }
        public int SentCount { get; set; }
        public DateTime CreatedTimeStamp { get; set; }

        //public List<Tuple<string,string>> GetListOfPropertiesWithValues()
        //{
        //    var propertiesList = new List<Tuple<string, string>>();
        //    propertiesList.Add(Tuple.Create("ReceiverCount", ReceiverCount.ToString()));
        //    return propertiesList;
        //}
    }
}
