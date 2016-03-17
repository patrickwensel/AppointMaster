using AppointMaster.Resources;
using AppointMaster.Services;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Services;

namespace AppointMaster.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; RaisePropertyChanged(() => IsChecked); }
        }

        private string _baseAPIAddress;
        public string BaseAPIAddress
        {
            get { return _baseAPIAddress; }
            set
            {
                _baseAPIAddress = value;
                RaisePropertyChanged(() => BaseAPIAddress);
            }
        }


        private CheckInModel _selectedCheckInModel;
        public CheckInModel SelectedCheckInModel
        {
            get { return _selectedCheckInModel; }
            set
            {
                _selectedCheckInModel = value;

                RaisePropertyChanged(() => SelectedCheckInModel);
            }
        }

        public MvxCommand ShowCheckInCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CheckInViewModel>());
            }
        }

        public MvxCommand SaveCommand
        {
            get
            {
                return new MvxCommand(() => Save());
            }
        }

        public MvxCommand BackCommand
        {
            get
            {
                return new MvxCommand(() => Back());
            }
        }

        public ObservableCollection<CheckInModel> Items { get; set; }

        public event EventHandler<string> SendMessage;

        public SettingsViewModel()
        {
            try
            {
                BaseAPIAddress = "http://ppgservices-001-site6.ctempurl.com/api/v1/";

                Items = new ObservableCollection<CheckInModel>();
                Items.Add(new CheckInModel { Name = AppResources.Digit_Selection, IsDigitModel = false });
                Items.Add(new CheckInModel { Name = AppResources.Appointment_List, IsDigitModel = false });

                SelectedCheckInModel = Items.Where(x => x.Name == AppResources.Appointment_List).FirstOrDefault();
                SelectedCheckInModel.IsDigitModel = true;

                byte[] bytesDemo = DataHelper.GetInstance().SecureStorage.Retrieve("DemoMode");
                bool isDemoMode = Convert.ToBoolean(Encoding.UTF8.GetString(bytesDemo, 0, bytesDemo.Length));
                IsChecked = isDemoMode;

                var bytes = DataHelper.GetInstance().SecureStorage.Retrieve("BaseAPI");

                var checkInModelBytes = DataHelper.GetInstance().SecureStorage.Retrieve("CheckInModel");
                string checkInModel = Encoding.UTF8.GetString(checkInModelBytes, 0, checkInModelBytes.Length);
                Items.Where(x => x.IsDigitModel).FirstOrDefault().IsDigitModel = false;
                SelectedCheckInModel = Items.Where(x => x.Name == checkInModel).FirstOrDefault();
                SelectedCheckInModel.IsDigitModel = true;

                BaseAPIAddress = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
            }
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(BaseAPIAddress))
            {
                if (SendMessage != null)
                {
                    SendMessage(null, null);
                }
                return;
            }

            try
            {
                DataHelper.GetInstance().IsDemoMode = IsChecked;

                DataHelper.GetInstance().SecureStorage.Store("DemoMode", Encoding.UTF8.GetBytes(IsChecked.ToString()));
                DataHelper.GetInstance().SecureStorage.Store("BaseAPI", Encoding.UTF8.GetBytes(BaseAPIAddress));
                DataHelper.GetInstance().SecureStorage.Store("CheckInModel", Encoding.UTF8.GetBytes(SelectedCheckInModel.Name));

                Back();
            }
            catch (Exception ex)
            {
            }
        }

        private void Back()
        {
            try
            {
                DataHelper.GetInstance().SecureStorage.Retrieve("UserName");

                ShowViewModel<MainViewModel>();
            }
            catch (Exception)
            {
                ShowViewModel<LoginViewModel>();
            }
        }

        public class CheckInModel : INotifyPropertyChanged
        {
            public string Name { get; set; }

            private bool _isDigitModel;
            public bool IsDigitModel { get { return _isDigitModel; } set { _isDigitModel = value; OnPropertyChanged("IsDigitModel"); } }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
