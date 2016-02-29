using System.ComponentModel;

namespace AppointMaster.Models
{
    public class PatientInfo: INotifyPropertyChanged
    {
        public string PatientName { get; set; }

        public string PatientGender { get; set; }

        public string Breed { get; set; }

        public string Birth { get; set; }

        public string Image { get; set; }

        public bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

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
