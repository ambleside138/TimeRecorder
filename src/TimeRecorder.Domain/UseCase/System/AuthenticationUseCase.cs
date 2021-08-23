using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.System;

namespace TimeRecorder.Domain.UseCase.System
{
    public class AuthenticationUseCase
    {
        private readonly IAccountRepository _AccountRepository;

        public AuthenticationUseCase(IAccountRepository accountRepository)
        {
            _AccountRepository = accountRepository;
        }

        public LoginStatus TrySignin()
        {
            return _AccountRepository.GetLoginStatus();
        }
    }
}
