using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Common
{
    public class CorrespondenceResult
    {
        public string ResultMessage { get; set; }
        public CorrespondenceResultType ResultType { get; set; }
    }

    public enum CorrespondenceResultType
    {
        Ok,
        Failed,
    }
}
