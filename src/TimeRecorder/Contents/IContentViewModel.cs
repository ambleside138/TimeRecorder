using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.NavigationRail;

namespace TimeRecorder.Contents
{
    /// <summary>
    /// タブごとのコンテンツに対応するViewModelを表します
    /// </summary>
    public interface IContentViewModel
    {
        NavigationIconButtonViewModel NavigationIcon { get; }

        ReactivePropertySlim<bool> IsSelected { get; }
    }
}
