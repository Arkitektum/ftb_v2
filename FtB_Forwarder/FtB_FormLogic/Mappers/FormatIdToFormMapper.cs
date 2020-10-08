﻿using FtB_Common.FormLogic;
using FtB_Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FtB_FormLogic
{
    public class FormatIdToFormMapper
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<FormatIdToFormMapper> _log;

        public FormatIdToFormMapper(IServiceProvider services, ILogger<FormatIdToFormMapper> log)
        {
            Debug.WriteLine("Constructor FormatIdToFormMapper");
            _services = services;
            _log = log;
        }

        public IFormLogic<T,U> GetForm<T,U>(string formatId, FormLogicProcessingContext processingContext)
        {
            //Retrieves classes implementing IForm, having FormDataFormatAttribute and filtering by its DataFormatId
            _log.LogDebug($"{GetType().Name}: GetForm for formatId {formatId}....");
            var type = typeof(IFormLogic<T, U>);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                //.Where(p => type.IsAssignableFrom(p))
                .Where(t => t.IsDefined(typeof(FormDataFormatAttribute), true))
                .Where(t => t.GetCustomAttribute<FormDataFormatAttribute>().DataFormatId == formatId &&  
                            t.GetCustomAttribute<FormDataFormatAttribute>().ProcessingContext == processingContext);

            object formInstance = null;
            if (types.Count() > 0)
            {
                //Resolves an instance of the class
                var formType = types.FirstOrDefault();
                formInstance = _services.GetService(formType);
            }

            return formInstance as IFormLogic<T, U>;
        }
    }
}