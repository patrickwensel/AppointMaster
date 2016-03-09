﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Models
{
    public class PatientModel : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int? SpeciesID { get; set; }

        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }

        public string Breed { get; set; }
        public string Gender { get; set; }
        public byte[] Logo { get; set; }
        public DateTime? Birthdate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DisplayPatientModel : PatientModel
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; OnPropertyChanged("IsChecked"); }
        }

        public string Species { get; set; }

        //test data
        public string ImgLogo { get; set; }
    }
}
