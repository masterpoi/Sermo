using System;
using System.ComponentModel;

namespace Sermo
{
    public class Seat : INotifyPropertyChanged
    {
        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                RaisePropertyChanged("FirstName");
            }
        }
        private string _LastName;
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        private bool _Selected;
        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                RaisePropertyChanged("Selected");
            }
        }

        public float TableTop { get; set; }
        public float TableLeft { get; set; }
        public float TableWidth { get; set; }
        public float TableHeight { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
