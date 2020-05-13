using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility.Exceptions
{
    // どこまで例外を設けるべきか...　なやむ

    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            :base(message)
        {

        }
    }
}
