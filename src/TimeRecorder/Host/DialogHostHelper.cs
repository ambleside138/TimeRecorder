using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Host;

internal static class DialogHostHelper
{
    public const string HostDialogIdentifier = "RootDialog";

    public static Task<object> Show(object content)
    {
        return DialogHost.Show(content, HostDialogIdentifier);
    }
}
