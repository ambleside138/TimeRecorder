using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Hospitals
{
    public class Hospital
    {
        public Identity<Hospital> Id { get; set; }

        public string Name { get; set; }

        public string KanaName { get; set; }

        public static Hospital Empty => new Hospital(Identity<Hospital>.Empty, "未選択", "ミセンタク");

        public Hospital(Identity<Hospital> id, string name, string kanaName)
        {
            Id = id;
            Name = name;
            KanaName = kanaName;
        }
    }
}
