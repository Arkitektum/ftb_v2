using System;
using System.Collections.Generic;
using System.Text;

namespace Distributor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DistributorTypeAttribute : Attribute
    {
        public string Id { get; set; }
    }
}
