using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Distributor
{
    public class DistributorFactory : IDistributorFactory
    {
        private readonly IEnumerable<IDistributor> _distributors;
        private readonly IOptions<DistributorSettings> _options;

        public DistributorFactory(IEnumerable<IDistributor> distributors, IOptions<DistributorSettings> options)
        {
            _distributors = distributors;

            if (string.IsNullOrEmpty(options?.Value?.DistributorType))
                throw new ArgumentException("Ensure that DistributorSettings is configured");

            _options = options;
        }

        public IDistributor GetDistributor()
        {
            foreach (var distributor in _distributors)
            {
                var type = distributor.GetType();
                var attribute = type.GetCustomAttributes(typeof(DistributorTypeAttribute), false).FirstOrDefault() as DistributorTypeAttribute;

                if (attribute.Id.Equals(_options.Value.DistributorType))
                    return distributor;
            }

            return null;
        }
    }
}
