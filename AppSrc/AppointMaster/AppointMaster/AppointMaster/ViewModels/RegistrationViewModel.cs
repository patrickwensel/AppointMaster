using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.ViewModels
{
    public class RegistrationViewModel : MvxViewModel
    {
        private int _selectedPageIndex;
        public int SelectedPageIndex
        {
            get { return _selectedPageIndex; }
            set
            {
                if (value == _selectedPageIndex) return;
                _selectedPageIndex = value;
                RaisePropertyChanged(() => SelectedPageIndex);
            }
        }
    }
}
