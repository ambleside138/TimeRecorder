using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Configurations.Items;

class ImportParamConfig : ConfigItemBase
{
    public string Param { get; set; }
    internal override ConfigKey Key => ConfigKey.ImportParam;
}
