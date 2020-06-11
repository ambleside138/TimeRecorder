using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.UseCase.System;

namespace TimeRecorder.Configurations
{
    enum ConfigKey
    {
        Theme,
    };

    class ConfigurationManager
    {
        public static ConfigurationManager Instance { get; } = new ConfigurationManager();

        private readonly ConfigurationUseCase _ConfigurationUseCase;

        private ConfigurationItem[] _ConfigurationItems;

        private ConfigurationManager()
        {
            _ConfigurationUseCase = new ConfigurationUseCase(ContainerHelper.Resolver.Resolve<IConfigurationRepository>());

            Load();
        }

        public void Load()
        {
            _ConfigurationItems = _ConfigurationUseCase.GetConfigurationItems();
        }

        public T GetConfiguration<T>(ConfigKey key) where T : class
        {
            var target = _ConfigurationItems.FirstOrDefault(c => c.Key == key.ToString());

            return target?.GetConfigValue<T>();
        }

        public void SetConfiguration<T>(ConfigKey key, T value)
        {
            var item = new ConfigurationItem(key.ToString());
            item.SetConfigValue(value);

            _ConfigurationUseCase.SetConfiguration(item);
            Load();
        }
    }
}
