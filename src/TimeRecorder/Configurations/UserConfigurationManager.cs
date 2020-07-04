using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Configurations.Items;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Domain.UseCase.System;

namespace TimeRecorder.Configurations
{


    class UserConfigurationManager
    {
        private static UserConfigurationManager _Instance;

        public static UserConfigurationManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new UserConfigurationManager();

                return _Instance;
            }

        }

        private readonly ConfigurationUseCase _ConfigurationUseCase;

        private ConfigurationItem[] _ConfigurationItems;

        private UserConfigurationManager()
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

        public void SetConfiguration<T>(T value) where T : ConfigItemBase
        {
            var item = new ConfigurationItem(value.Key.ToString());
            item.SetConfigValue(value);

            _ConfigurationUseCase.SetConfiguration(item);
            Load();
        }
    }
}
