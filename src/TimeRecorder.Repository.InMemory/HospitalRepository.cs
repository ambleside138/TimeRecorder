using System;
using TimeRecorder.Domain.Domain.Hospitals;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Repository.InMemory
{
    public class HospitalRepository : IHospitalRepository
    {
        public Hospital[] SelectAll()
        {
            return new Hospital[]
            {
                new Hospital(new Identity<Hospital>(1), "朽木病院","クチキビョウイン"),
                new Hospital(new Identity<Hospital>(2), "黒崎眼科", "クロサキガンカ"),
                new Hospital(new Identity<Hospital>(3), "井上病院", "イノウエビョウイン"),
                new Hospital(new Identity<Hospital>(4), "石田クリニック", "イシダクリニック"),
            };
        }
    }
}
