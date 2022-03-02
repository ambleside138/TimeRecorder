using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Configurations.Items;

class WorkingHourImportApiUrlConfig : ConfigItemBase
{
    public string URL { get; set; }
    internal override ConfigKey Key => ConfigKey.WorkingHourImportApiUrl;
}
