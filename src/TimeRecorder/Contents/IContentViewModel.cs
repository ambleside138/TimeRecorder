using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.NavigationRail.ViewModels;

namespace TimeRecorder.Contents
{
    /// <summary>
    /// タブごとのコンテンツに対応するViewModelを表します
    /// </summary>
    public interface IContentViewModel
    {
        NavigationIconButtonViewModel NavigationIcon { get; }


    }
}
