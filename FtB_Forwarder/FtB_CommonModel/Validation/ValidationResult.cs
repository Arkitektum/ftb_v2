using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FtB_CommonModel.Validation
{
    public class ValidationResult
    {
        /// <summary>
        /// Number of errors
        /// </summary>
        public int Errors;
        /// <summary>
        /// Number of warnings
        /// </summary>
        public int Warnings;

        public List<ValidationMessage> messages
        {
            get { return _messages.ToList(); }
        }

        public List<ValidationRule> rulesChecked
        {
            get { return _rulesChecked.ToList(); }
        }



        private readonly ConcurrentBag<ValidationMessage> _messages;

        private readonly ConcurrentBag<ValidationRule> _rulesChecked;

        public ValidationResult()
        {
            Warnings = 0;
            Errors = 0;
            _messages = new ConcurrentBag<ValidationMessage>();
            _rulesChecked = new ConcurrentBag<ValidationRule>();
        }

        public ValidationResult(int warningCount, int errorCount)
        {
            Warnings = warningCount;
            Errors = errorCount;
            _messages = new ConcurrentBag<ValidationMessage>();
            _rulesChecked = new ConcurrentBag<ValidationRule>();
        }

        public void AddRule(string skjema, string id, string checklistReference, string message, string xpath, string messagetype, string preCondition)
        {
            var xpathField = ReplaceFormWithSkjema(xpath, skjema);
            var preConditionField = ReplaceFormWithSkjema(preCondition, skjema);

            _rulesChecked.Add(new ValidationRule
            {
                id = id,
                messagetype = messagetype,
                message = message,
                checklistReference = checklistReference,
                preCondition = preConditionField,
                xpathField = xpathField
            });

        }

        internal string ReplaceFormWithSkjema(string xpathField, string skjema)
        {
            string skjemaRefpath = "";
            if (!string.IsNullOrEmpty(xpathField))
            {
                if (xpathField.Contains("form"))
                {
                    skjemaRefpath = xpathField?.Replace("form", xpathField.Contains("form/") ? $"{skjema}" : $"{skjema}/");
                }
                else
                {
                    skjemaRefpath = xpathField;
                }
            }

            return skjemaRefpath;
        }

        public void AddRule(string id, string message, string preCondition, string messagetype, string checklistReference)
        {
            _rulesChecked.Add(new ValidationRule { id = id, messagetype = messagetype, message = message, checklistReference = checklistReference, preCondition = preCondition });
        }
        public ValidationRule GetRule(string id)
        {
            return _rulesChecked?.FirstOrDefault(r => r.id == id);// .Find(r => r.id == id);
        }
        //----
        public void AddMessage(string ruleId, string xpathField = null, string skjema = null, string[] ruleMessagesParameters = null)
        {
            var rule = GetRule(ruleId);
            if (rule == null)
            {
                _messages.Add(new ValidationMessage { messagetype = "WARNING", message = "Meldingskode ikke funnet", reference = ruleId, xpathField = "" });
                return;
            }

            var message = ruleMessagesParameters != null ? string.Format(rule.message, ruleMessagesParameters) : rule.message;
            var skjemaRefpath = string.Empty;
            if (!string.IsNullOrEmpty(xpathField))
            {
                if (xpathField.Contains("form"))
                {
                    skjemaRefpath = xpathField?.Replace("form/", skjema + "/");
                }
                else
                {
                    skjemaRefpath = xpathField;
                }
            }

            if (rule.messagetype.Trim().Equals("ERROR", StringComparison.InvariantCultureIgnoreCase))
            {
                Interlocked.Increment(ref Errors);
                _messages.Add(new ValidationMessage { messagetype = "ERROR", message = message, reference = ruleId, xpathField = skjemaRefpath });
            }
            if (rule.messagetype.Trim().Equals("WARNING", StringComparison.InvariantCultureIgnoreCase))
            {
                Interlocked.Increment(ref Warnings);
                _messages.Add(new ValidationMessage { messagetype = "WARNING", message = message, reference = ruleId, xpathField = skjemaRefpath });
            }
        }
        public void AddMessage(string ruleId, string[] ruleMessagesParameters = null)
        {
            var rule = GetRule(ruleId);
            if (rule == null)
            {
                _messages.Add(new ValidationMessage { messagetype = "WARNING", message = "Meldingskode ikke funnet", reference = ruleId, xpathField = "" });
                return;
            }

            var message = ruleMessagesParameters != null ? string.Format(rule.message, ruleMessagesParameters) : rule.message;

            if (rule.messagetype.Trim().Equals("ERROR", StringComparison.InvariantCultureIgnoreCase))
            {
                Interlocked.Increment(ref Errors);
                _messages.Add(new ValidationMessage { messagetype = "ERROR", message = message, reference = ruleId, xpathField = rule.xpathField });
            }
            if (rule.messagetype.Trim().Equals("WARNING", StringComparison.InvariantCultureIgnoreCase))
            {
                Interlocked.Increment(ref Warnings);
                _messages.Add(new ValidationMessage { messagetype = "WARNING", message = message, reference = ruleId, xpathField = rule.xpathField });
            }
        }

        //----
        public void AddError(string message, string reference, string xpathField)
        {
            Interlocked.Increment(ref Errors);
            _messages.Add(new ValidationMessage { messagetype = "ERROR", message = message, reference = reference, xpathField = xpathField });
        }

        public void AddError(string message, string reference)
        {
            Interlocked.Increment(ref Errors);
            _messages.Add(new ValidationMessage { messagetype = "ERROR", message = message, reference = reference });
        }

        public void AddWarning(string message, string reference)
        {
            Interlocked.Increment(ref Warnings);
            _messages.Add(new ValidationMessage { messagetype = "WARNING", message = message, reference = reference });
        }

        public void AddWarning(string message, string reference, string xpathField)
        {
            Interlocked.Increment(ref Warnings);
            _messages.Add(new ValidationMessage { messagetype = "WARNING", message = message, reference = reference, xpathField = xpathField });
        }

        public bool HasErrors()
        {
            return !IsOk();
        }

        public bool IsOk()
        {
            return Errors == 0;
        }

        public override string ToString()
        {
            var toStr = string.Empty;

            foreach (var msg in _messages)
                toStr = toStr + $"{msg.messagetype}: {msg.message}, iht. {msg.reference}; ";
            return toStr;
        }
    }

    public class ValidationMessage
    {
        public string message;
        public string messagetype;
        public string reference;
        public string xpathField;
    }

    public class ValidationRule
    {
        public string id;
        public string message;
        public string messagetype;
        public string preCondition;
        public string checklistReference;
        public string xpathField;

    }
}
