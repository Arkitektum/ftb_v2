using System.ServiceModel.Channels;

namespace AltinnWebServices.Bindings
{
    public interface IBindingFactory
    {
        Binding GetBindingFor(BindingType bindingType);
    }
}