using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.System;

public interface IConfigurationRepository : IRepository
{
    void UpdateConfiguration(ConfigurationItem item);

    ConfigurationItem[] SelectAll();
}
