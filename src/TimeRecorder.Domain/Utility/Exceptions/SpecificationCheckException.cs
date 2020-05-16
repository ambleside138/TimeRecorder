using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TimeRecorder.Domain.Utility.Exceptions
{
    public class SpecificationCheckException : Exception
    {
        public SpecificationCheckException(ValidationResult validationResult)
            :base(validationResult.ErrorMessage)
        {
            ValidationResult = validationResult;
        }

        public ValidationResult ValidationResult { get; }
    }
}
