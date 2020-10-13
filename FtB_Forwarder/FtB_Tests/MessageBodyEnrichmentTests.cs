using Altinn.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Tests
{
    [TestClass]
    public class MessageBodyEnrichmentTests
    {
        //[TestMethod]
        //public void EnrichBody_validInput()
        //{
        //    var messageBody = "Dette er en test. Referanse til noko er {prefillId}";
        //    var messageTitle = "Ich bin title";
        //    var messageSummary = "Oppsummering ja";

        //    MessageDataType messageType = null;

        //    messageType = new MessageDataType((values, messageBody) =>
        //    {
        //        foreach (var item in values)
        //        {
        //            messageBody = messageBody.Replace(item.Key, item.Value, StringComparison.InvariantCultureIgnoreCase);
        //        }
        //    })
        //    {
        //        MessageBody = messageBody,
        //        MessageSummary = messageSummary,
        //        MessageTitle = messageTitle
        //    };

        //    var kv = new List<KeyValuePair<string, string>>();
        //    kv.Add(new KeyValuePair<string, string>("{prefillId}", "12345611122"));

        //    messageType.EnrichBodyWith(kv);

        //    var mb = messageType.MessageBody;

        //}

    }
}
