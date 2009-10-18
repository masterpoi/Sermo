using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Sermo
{
    [DataContract]
    public class Seat : INotifyPropertyChanged
    {
        private string _FirstName;
        [DataMember]
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
        [DataMember]
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

        [DataMember]
        public string Function { get; set; }
        [DataMember]
        public string Company { get; set; }

        private Color? _Color;
        public Color? Color
        {
            get
            {
                return _Color;
            }
            set
            {
                _Color = value;
                RaisePropertyChanged("Color");
                RaisePropertyChanged("Brush");
            }
        }

        public Brush Brush
        {
            get
            {
                if (!Color.HasValue)
                    return new SolidColorBrush(Colors.Transparent);
                return new SolidColorBrush(Color.Value);
            }
        }
            


        private SeatState _Selected = SeatState.Sitting;
        [DataMember]
        public SeatState State
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                if (value == SeatState.Sitting)
                    Color = null;
                RaisePropertyChanged("State");
            }
        }
        [DataMember]
        public float TableTop { get; set; }
        [DataMember]
        public float TableLeft { get; set; }
        [DataMember]
        public float TableWidth { get; set; }
        [DataMember]
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
