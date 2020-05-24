using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeRecorder.Domain.Utility.Exceptions
{
    public class SpecificationCheckException : Exception
    {
        public SpecificationCheckException(ValidationResult validationResult)
            :base("[エラー] " + validationResult.ErrorMessage)
        {
            ValidationResult = validationResult;
        }

        public SpecificationCheckException(string message)
            : this(new ValidationResult(message)) { }

        public ValidationResult ValidationResult { get; }
    }
}
