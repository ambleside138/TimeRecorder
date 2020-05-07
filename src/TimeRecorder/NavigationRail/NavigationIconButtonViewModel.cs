using Livet;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.NavigationRail.ViewModels
{
    public class NavigationIconButtonViewModel : ViewModel
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

        public bool IsSelected
        {
            get => _IsSelected;
            set => RaisePropertyChangedIfSet(ref _IsSelected, value);
        }


    }
}
