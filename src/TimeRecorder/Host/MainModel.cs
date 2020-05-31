using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.UseCase.System;
using TimeRecorder.Repository.SQLite;

namespace TimeRecorder.Host
{
    class MainModel
    {
        private readonly CheckStatusUseCase _CheckStatusUseCase;

        public MainModel()
        {
            _CheckStatusUseCase = new CheckStatusUseCase(ContainerHelper.Resolver.Resolve<IHealthChecker>());
        }

        public string CheckHealth()
        {
            try
            {
                var result = _CheckStatusUseCase.CheckSystemStatus();

                switch (result)
                {
                    case SystemStatus.OK:
                        return "";

                    case SystemStatus.InvalidVersion:
                        Setup.VersionUp();
                        return "ローカルDBファイルのバージョンアップを行いました";

                    case SystemStatus.NotInitialized:
                        Setup.CreateDatabaseFile();
                        return "新規インストールされたためローカルDBファイルを作成しました";

                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}
