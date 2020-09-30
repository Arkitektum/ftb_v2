using System.ServiceModel;
using System.ServiceModel.Channels;

namespace AltinnWebServices.Bindings
{
    public class BasicBindingProvider : IBinding
    {
        public Binding CreateBinding()
        {
            var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);

            return binding;
        }
    }
}