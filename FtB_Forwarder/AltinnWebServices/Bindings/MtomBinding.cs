using System.ServiceModel.Channels;
using System.Text;
using WcfCoreMtomEncoder;

namespace AltinnWebServices.Bindings
{
    public class MtomBindingProvider : IBinding
    {
        public Binding CreateBinding()
        {
            var encoding = new MtomMessageEncoderBindingElement(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8));

            var transport = new HttpsTransportBindingElement();
            transport.MaxReceivedMessageSize = int.MaxValue;
            transport.MaxBufferSize = int.MaxValue;
            transport.AllowCookies = false;

            var customBinding = new CustomBinding(encoding, transport);

            return customBinding;
        }
    }
}
