using Livet;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.NavigationRail;

public class NavigationIconButtonViewModel : ViewModel, INavigationItem
{

    private string _Title;

    public string Title
    {
        get => _Title;
        set => RaisePropertyChangedIfSet(ref _Title, value);
    }


    private string _IconKey;

    public string IconKey
    {
        get => _IconKey;
        set => RaisePropertyChangedIfSet(ref _IconKey, value);
    }

    private bool _IsSelected;

    public virtual bool IsSelected
    {
        get => _IsSelected;
        set => RaisePropertyChangedIfSet(ref _IsSelected, value);
    }


    #region IsSelectable変更通知プロパティ
    private bool _IsSelectable = true;

    public virtual bool IsSelectable
    {
        get => _IsSelectable;
        set => RaisePropertyChangedIfSet(ref _IsSelectable, value);
    }
    #endregion

}
