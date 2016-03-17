using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointMaster.Models
{
    public class SpeciesModel: INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ClinicID { get; set; }
        public int SpeciesID { get; set; }
        public bool PrimaryDisplay { get; set; }
        public byte[] Logo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class DisplaySpeciesModel : SpeciesModel
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public string ImgLogo { get; set; }
    }
}
