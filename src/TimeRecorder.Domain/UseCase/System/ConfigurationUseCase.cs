using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.System;

namespace TimeRecorder.Domain.UseCase.System
{
    public class ConfigurationUseCase
    {
        private readonly IConfigurationRepository _ConfigurationRepository;

        public ConfigurationUseCase(IConfigurationRepository configurationRepository)
        {
            _ConfigurationRepository = configurationRepository;
        }

        public void SetConfiguration(ConfigurationItem configurationItem)
        {
            _ConfigurationRepository.UpdateConfiguration(configurationItem);
        }

        public ConfigurationItem[] GetConfigurationItems()
        {
            return _ConfigurationRepository.SelectAll();
        }
    }
}
