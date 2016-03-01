using MvvmCross.Core.ViewModels;

namespace AppointMaster.ViewModels
{
    public class CheckedInViewModel:MvxViewModel
    {
        public MvxCommand ShowMainCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<MainViewModel>());
            }
        }

        public MvxCommand ShowRegistertionCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<RegistrationViewModel>());
            }
        }
    }
}
