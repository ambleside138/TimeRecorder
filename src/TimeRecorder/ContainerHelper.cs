using MicroResolver;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Repository.SQLite.Tasks;
using TimeRecorder.Repository.SQLite.Tracking;

namespace TimeRecorder
{
    class ContainerHelper
    {
        public static ObjectResolver Resolver { get; private set; }

        public static void Setup()
        {
            // Create a new container
            var resolver = ObjectResolver.Create();

            // Register interface->type map, default is transient(instantiate every request)
            resolver.Register<IWorkTaskRepository, SQLiteWorkTaskRepository>(Lifestyle.Singleton);
            resolver.Register<IWorkingTimeRangeRepository, SQLiteWorkingTimeRangeRepository>(Lifestyle.Singleton);
            resolver.Register<IDailyWorkRecordQueryService, SQLiteDailyWorkRecordQueryService>(Lifestyle.Singleton);
            resolver.Register<IWorkingTimeQueryService, SQLiteWorkingTimeQueryService>(Lifestyle.Singleton);

            // You can configure lifestyle - Transient, Singleton or Scoped
            //resolver.Register<ILogger, MailLogger>(Lifestyle.Singleton);

            // Compile and Verify container(this is required step)
            resolver.Compile();

            Resolver = resolver;

            // Get instance from container
            //var userRepository = resolver.Resolve<IUserRepository>();
            //var logger = resolver.Resolve<ILogger>();
        }
    }
}
