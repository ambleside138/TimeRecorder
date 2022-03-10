using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Configurations.Items;

class UseTodoConfig : ConfigItemBase
{
    public bool UseTodo { get; set; }
    internal override ConfigKey Key => ConfigKey.UseTodo;
}
