using System.ComponentModel;

namespace TimeRecorder.NavigationRail
{
    public interface INavigationItem : INotifyPropertyChanged
    {
        /// <summary>
        /// ユーザが項目を選択可能かどうかを取得、設定します
        /// </summary>
        bool IsSelectable { get; set; }

        /// <summary>
        /// この項目が選択されているかどうかを取得、設定します
        /// </summary>
        bool IsSelected { get; set; }
    }
}
