using System.ServiceModel.Channels;

namespace Altinn2.Adapters.Bindings
{
    public interface IBinding
    {
        Binding CreateBinding();
    }
}
