using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;

namespace AltinnWebServices.Bindings
{
    public class BindingFactory : IBindingFactory
    {
        private readonly IEnumerable<IBinding> _bindings;

        public BindingFactory(IEnumerable<IBinding> bindings)
        {
            _bindings = bindings;
        }
        public Binding GetBindingFor(BindingType bindingType)
        {
            Binding binding = null;
            switch (bindingType)
            {
                case BindingType.Mtom:
                    binding = _bindings.Where(b => b is MtomBindingProvider).FirstOrDefault()?.CreateBinding();
                    break;
                case BindingType.Normal:
                    //What is normal? :thinking:
                    break;
                default:
                    break;
            }
            return binding;
        }
    }
}
