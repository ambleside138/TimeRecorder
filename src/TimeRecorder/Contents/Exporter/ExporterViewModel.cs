using Livet;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.NavigationRail.ViewModels;

namespace TimeRecorder.Contents.Exporter
{
    public class ExporterViewModel : ViewModel, IContentViewModel
    {
        public NavigationIconButtonViewModel NavigationIcon => new NavigationIconButtonViewModel { Title = "出力", IconKey = "FileExport" };

    }
}
