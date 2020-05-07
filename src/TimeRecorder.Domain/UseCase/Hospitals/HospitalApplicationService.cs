using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Hospitals;

namespace TimeRecorder.UseCase.Hospitals
{
    /// <summary>
    /// 病院（＝案件） に関するPresentation層との相互作用を実装します
    /// </summary>
    public class HospitalApplicationService
    {
        private readonly IHospitalRepository _HospitalRepository;

        public HospitalApplicationService(IHospitalRepository hospitalRepository)
        {
            _HospitalRepository = hospitalRepository;
        }

        public Hospital[] GetHospitals()
        {
            return _HospitalRepository.SelectAll();
        }
    }
}
