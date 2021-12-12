using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.System
{
    public class LoginStatus
    {
        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Account => string.IsNullOrEmpty(Email) ? "" : Email.Split('@', StringSplitOptions.None).FirstOrDefault();
        public string PhotoUrl { get; set; }
    }
}
