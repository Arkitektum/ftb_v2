using System.ServiceModel.Channels;

namespace AltinnWebServices.Bindings
{
    public interface IBinding
    {
        Binding CreateBinding();
    }
}
