using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PreProsessor.Validation
{
    public class Validator
    {
        private readonly ILogger<Validator> _logger;

        public Validator(ILogger<Validator> logger, IValdiationFormMapper valdiationFormMapper)
        {
            _logger = logger;
        }
        public string Validate(QueuedForm queuedForm)
        {
            return string.Empty;
        }
    }
}
