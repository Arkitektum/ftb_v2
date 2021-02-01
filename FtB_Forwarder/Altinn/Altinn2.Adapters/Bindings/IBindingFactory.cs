using System.ServiceModel.Channels;

namespace Altinn2.Adapters.Bindings
{
    public interface IBindingFactory
    {
        Binding GetBindingFor(BindingType bindingType);
    }
}