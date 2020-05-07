using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Hospitals
{
    public interface IHospitalRepository
    {
        Hospital[] SelectAll();
    }
}
