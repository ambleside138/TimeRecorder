using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.UseCase.Tasks;
using TimeRecorder.Domain.UseCase.Tracking;
using TimeRecorder.Domain.UseCase.Tracking.Reports;
using TimeRecorder.Driver.CsvDriver;
using TimeRecorder.Driver.CsvDriver.Import;
using TimeRecorder.Repository.Firebase.System;
using TimeRecorder.Repository.Firebase.Todo;
using TimeRecorder.Repository.GoogleAPI.Calendar;
using TimeRecorder.Repository.InMemory;
using TimeRecorder.Repository.SQLite.Clients;
using TimeRecorder.Repository.SQLite.Products;
using TimeRecorder.Repository.SQLite.System;
using TimeRecorder.Repository.SQLite.Tasks;
using TimeRecorder.Repository.SQLite.Tracking;
using TimeRecorder.Repository.SQLite.Tracking.Reports;
using TimeRecorder.Repository.SQLite.WorkProcesses;

namespace TimeRecorder
{
    internal static class ContainerHelper
    {
        public static ServiceProvider Provider { get; private set; }

        public static T GetRequiredService<T>() => Provider.GetRequiredService<T>();


        public static void Setup()
        {
            IServiceCollection services = new ServiceCollection()
                                              .AddMessagePipeServices()
                                              .AddRepositoryServices();

            // フォームのインスタンスをDIで生成する場合はアプリケーションのフォームを登録する
            services.AddTransient<Domain.UseCase.Todo.TodoItemUseCase>()
                    .AddTransient<Domain.UseCase.System.AuthenticationUseCase>();

            // サービスプロバイダーを生成する
            Provider = services.BuildServiceProvider();
        }

        /// <summary>
        /// MessagePipe を使用するためのサービスを生成します。
        /// </summary>
        /// <returns></returns>
        private static IServiceCollection AddMessagePipeServices(this IServiceCollection services)
        {

            // MessagePipe の標準サービスを登録する
            return services.AddMessagePipe(options =>
                                           {
                                               // 全てのメッセージに適用したいフィルタはグローバルフィルタとして定義するとよい
                                               //options.AddGlobalMessageHandlerFilter(typeof(SampleFilter<>));
                                           }
                                           )
                            // 使用するメッセージを登録する
                            .AddSingleton(typeof(MessagePipe.IPublisher<>), typeof(MessageBroker<>))
                            .AddSingleton(typeof(MessagePipe.ISubscriber<>), typeof(MessageBroker<>));

        }

        private static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            return services.AddSingleton<IWorkTaskRepository, SQLiteWorkTaskRepository>()
                           .AddSingleton<IWorkProcessRepository, SQLiteWorkProcessRepository>()
                           .AddSingleton<IClientRepository, SQLiteClientRepository>()
                           .AddSingleton<IProductRepository, SQLiteProductRepository>()
                           .AddSingleton<IWorkingTimeRangeRepository, SQLiteWorkingTimeRangeRepository>()
                           .AddSingleton<IDailyWorkRecordQueryService, SQLiteDailyWorkRecordQueryService>()
                           .AddSingleton<IWorkingTimeQueryService, SQLiteWorkingTimeQueryService>()
                           .AddSingleton<IWorkTaskWithTimesQueryService, SQLiteWorkTaskWithTimesQueryService>()
                           .AddSingleton<IHealthChecker, SQLiteHealthChecker>()
                           .AddSingleton<IScheduledEventRepository, GoogleApiScheduledEventRepository>()
                           .AddSingleton<IConfigurationRepository, SQLiteConfigurationRepository>()
                           .AddSingleton<IWorkingHourRepository, SQLiteWorkingHoursRepository>()
                           .AddSingleton<ITodoItemRepository, FirestoreTodoItemRepository>()
                           .AddSingleton<IAccountRepository, FirebaseAccountRepository>()
                           .AddSingleton<IReportDriver, CsvReportDriver>()
                           .AddSingleton<IWorkingHourImportDriver, CsvWorkingHourImportDriver>();
        }
    }
}
