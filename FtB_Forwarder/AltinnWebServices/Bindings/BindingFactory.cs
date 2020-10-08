using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;

namespace Altinn2.Adapters.Bindings
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
                case BindingType.Basic:
                    binding = _bindings.Where(b => b is BasicBindingProvider).FirstOrDefault()?.CreateBinding();
                    break;
                default:
                    break;
            }
            return binding;
        }
    }
}
