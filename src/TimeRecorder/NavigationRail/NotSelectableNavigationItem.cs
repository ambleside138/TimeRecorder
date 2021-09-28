using Livet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.NavigationRail
{
    /// <summary>
    /// ヘッダやDividerなど選択不可能な項目を表します
    /// </summary>
    internal abstract class NotSelectableNavigationItem : NavigationIconButtonViewModel, INavigationItem
    {
        public override bool IsSelectable
        {
            get => false;
            set { }
        }

        public override bool IsSelected
        {
            get => false;
            set { }
        }
    }
}
