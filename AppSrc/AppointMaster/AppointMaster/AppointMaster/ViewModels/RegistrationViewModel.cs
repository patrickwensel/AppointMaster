﻿using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.ViewModels
{
    public class RegistrationViewModel : MvxViewModel
    {
        //private int _selectedPageIndex;
        //public int SelectedPageIndex
        //{
        //    get { return _selectedPageIndex; }
        //    set
        //    {
        //        if (value == _selectedPageIndex) return;
        //        _selectedPageIndex = value;
        //        RaisePropertyChanged(() => SelectedPageIndex);
        //    }
        //}

        private object _selectedTitle;
        public object SelectedTitle
        {
            get { return _selectedTitle; }
            set
            {
                _selectedTitle = value;
                RaisePropertyChanged(() => SelectedTitle);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }


        private string _streetAddress;
        public string StreetAddress
        {
            get { return _streetAddress; }
            set
            {
                _streetAddress = value;
                RaisePropertyChanged(() => StreetAddress);
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                RaisePropertyChanged(() => City);
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged(() => State);
            }
        }

        private string _zip;
        public string Zip
        {
            get { return _zip; }
            set
            {
                _zip = value;
                RaisePropertyChanged(() => Zip);
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                RaisePropertyChanged(() => Phone);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        public MvxCommand ShowCheckInCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<CheckInViewModel>());
            }
        }

        public ObservableCollection<TitleModel> TitleList { get; set; }

        public RegistrationViewModel()
        {
            TitleList = new ObservableCollection<TitleModel>();
            TitleList.Add(new TitleModel { Title = "Mr." });
            TitleList.Add(new TitleModel { Title = "Mrs." });
        }
    }

    public class TitleModel
    {
        public string Title { get; set; }
    }
}
