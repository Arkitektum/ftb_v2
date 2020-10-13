using Altinn2.Adapters.Bindings;
using AltinnWebServices.WS.Prefill;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ServiceModel;

namespace Altinn2.Adapters.WS.Prefill
{
    public class PrefillClient : IPrefillClient
    {
        private PreFillExternalBasicClient _client;
        private readonly IOptions<PrefillConnectionSettings> _connectionOptions;
        private readonly ILogger<PrefillClient> _log;
        //Move into separate class????
        private readonly string _externalBatchId = Guid.NewGuid().ToString();
        private readonly bool _doSaveFormTask = true;
        private readonly bool _doinstantiateFormTask = true;
        private readonly int? _caseId = null;
        private readonly string _instantitedOnBehalfOf = null;

        public PrefillClient(IBindingFactory bindingFactory, IOptions<PrefillConnectionSettings> connectionOptions, ILogger<PrefillClient> log)
        {
            _client = new PreFillExternalBasicClient(bindingFactory.GetBindingFor(BindingType.Mtom), new EndpointAddress(connectionOptions.Value.EndpointUrl));
            _connectionOptions = connectionOptions;
            _log = log;
        }

        public ReceiptExternal SendPrefill(PrefillFormTask prefillFormTask, DateTime? dueDate)
        {
            try
            {
                prefillFormTask.ServiceOwnerCode = _connectionOptions.Value.ServiceOwnerCode;

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
            catch (TimeoutException te)
            {
                _log.LogError(te, $"Timeout when communicating with Altinn 2 prefill service");
                _client.Abort();
                throw;
            }
            catch (CommunicationException ce)
            {
                _log.LogError(ce, $"Communication error occurred when communication with Altinn 2 prefill service");
                _client.Abort();
                throw;
            }

        }
    }
}
