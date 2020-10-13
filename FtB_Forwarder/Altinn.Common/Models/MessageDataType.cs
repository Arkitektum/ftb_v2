using System;
using System.Collections.Generic;

namespace Altinn.Common.Models
{
    public class MessageDataType
    {
        //private readonly Action<List<KeyValuePair<string, string>>, string> _enrichBodyAction;

        //public MessageDataType(Action<List<KeyValuePair<string, string>>, string> enrichBodyAction)
        //{
        //    _enrichBodyAction = enrichBodyAction;
        //}

        public string MessageTitle { get; set; }
        public string MessageSummary { get; set; }
        public string MessageBody { get; set; }

        //public void EnrichBodyWith(List<KeyValuePair<string, string>> kv)
        //{
        //    _enrichBodyAction?.Invoke(kv, MessageBody);
        //}
    }
}
