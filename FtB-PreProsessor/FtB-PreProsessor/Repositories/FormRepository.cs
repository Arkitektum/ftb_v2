using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_PreProsessor.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly ILogger<FormRepository> _logger;

        public FormRepository(ILogger<FormRepository> logger)
        {
            _logger = logger;
        }
        public void Save(object form)
        {
            _logger.LogInformation("Saved form to repository");
        }

        public object GetForm(string reference)
        {
            _logger.LogInformation("Retrieves form {0} to repository", reference);
            return reference;
        }
    }
}