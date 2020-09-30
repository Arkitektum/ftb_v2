using AltinnWebServices.Bindings;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;

namespace AltinnWebServices.WS.Prefill
{
    public class AltinnPrefillClient : IAltinnPrefillClient
    {
        private PreFillExternalBasicClient _client;
        private readonly IOptions<AltinnPrefillConnectionSettings> _connectionOptions;

        //Move into separate class????
        private readonly string _externalBatchId = Guid.NewGuid().ToString();
        private readonly bool _doSaveFormTask = true;
        private readonly bool _doinstantiateFormTask = true;
        private readonly int? _caseId = null;
        private readonly string _instantitedOnBehalfOf = null;
        
        public AltinnPrefillClient(IBindingFactory bindingFactory, IOptions<AltinnPrefillConnectionSettings> connectionOptions)
        {
            _client = new PreFillExternalBasicClient(bindingFactory.GetBindingFor(BindingType.Mtom), new EndpointAddress(connectionOptions.Value.EndpointUrl));
            _connectionOptions = connectionOptions;
        }

        public ReceiptExternal SendPrefill(PrefillFormTask prefillFormTask, DateTime? dueDate)
        {
            try
            {                
                var result = _client.SubmitAndInstantiatePrefilledFormTaskBasicAsync(_connectionOptions.Value.UserName,
                    _connectionOptions.Value.Password,
                    _externalBatchId,
                    prefillFormTask,
                    _doSaveFormTask,
                    _doinstantiateFormTask,
                    _caseId,
                    dueDate,
                    _instantitedOnBehalfOf).ConfigureAwait(true).GetAwaiter().GetResult();

                return result;
            }
            catch (TimeoutException)
            {
                _client.Abort();
                throw;
            }
            catch (CommunicationException)
            {
                _client.Abort();
                throw;
            }

        }
    }
}
