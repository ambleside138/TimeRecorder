using MicroResolver;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Driver.CsvExporter;
using TimeRecorder.Repository.GoogleAPI.Calendar;
using TimeRecorder.Repository.SQLite.Clients;
using TimeRecorder.Repository.SQLite.Products;
using TimeRecorder.Repository.SQLite.System;
using TimeRecorder.Repository.SQLite.Tasks;
using TimeRecorder.Repository.SQLite.Tracking;
using TimeRecorder.Repository.SQLite.Tracking.Reports;
using TimeRecorder.Repository.SQLite.WorkProcesses;

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
            resolver.Register<IWorkProcessRepository, SQLiteWorkProcessRepository>(Lifestyle.Singleton);
            resolver.Register<IClientRepository, SQLiteClientRepository>(Lifestyle.Singleton);
            resolver.Register<IProductRepository, SQLiteProductRepository>(Lifestyle.Singleton);
            resolver.Register<IWorkingTimeRangeRepository, SQLiteWorkingTimeRangeRepository>(Lifestyle.Singleton);
            resolver.Register<IDailyWorkRecordQueryService, SQLiteDailyWorkRecordQueryService>(Lifestyle.Singleton);
            resolver.Register<IWorkingTimeQueryService, SQLiteWorkingTimeQueryService>(Lifestyle.Singleton);
            resolver.Register<IWorkTaskWithTimesQueryService, SQLiteWorkTaskWithTimesQueryService>(Lifestyle.Singleton);
            resolver.Register<IHealthChecker, SQLiteHealthChecker>(Lifestyle.Singleton);
            resolver.Register<IScheduledEventRepository, GoogleApiScheduledEventRepository>(Lifestyle.Singleton);
            resolver.Register<IConfigurationRepository, SQLiteConfigurationRepository>(Lifestyle.Singleton);
            resolver.Register<IWorkingHourRepository, SQLiteWorkingHoursRepository>(Lifestyle.Singleton);

            resolver.Register<IReportDriver, CsvReportDriver>(Lifestyle.Singleton);

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
