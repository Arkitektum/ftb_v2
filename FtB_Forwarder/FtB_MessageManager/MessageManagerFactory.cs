using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtB_MessageManager
{
    public class MessageManagerFactory : IMessageManagerFactory
    {
        private readonly IEnumerable<IMessageManager> _messageManagers;
        private readonly IOptions<MessageManagerSettings> _options;

        public MessageManagerFactory(IEnumerable<IMessageManager> messageManagers, IOptions<MessageManagerSettings> options)
        {
            _messageManagers = messageManagers;
            _options = options;
            if (string.IsNullOrEmpty(options?.Value?.MessageManagerType))
                throw new ArgumentException("Ensure that MessageManagerSettings is configured");

            _options = options;
            _messageManagers = messageManagers;
        }
        public IMessageManager GetMessageManager()
        {
            foreach (var messageManager in _messageManagers)
            {
                var type = messageManager.GetType();
                var attribute = type.GetCustomAttributes(typeof(MessageManagerTypeAttribute), false).FirstOrDefault() as MessageManagerTypeAttribute;

                if (attribute.Id.Equals(_options.Value.MessageManagerType))
                    return messageManager;
            }

            return null;
        }
    }
}
