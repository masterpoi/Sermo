using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Sermo
{

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            


            //Initialize services
            Unloaded += (s, e) => StopService();
            StartService();



            Seat[] localseats = null;

            if (System.IO.File.Exists("config.xml"))
            {

                var ser = new XmlSerializer(typeof(Seat[]));
                using (var file = File.OpenRead(@"config.xml"))
                {
                    localseats = (Seat[])ser.Deserialize(file);

                }
                _SeatChannel.TransmitConfig(localseats);
            }
            

            _SeatService.SomeoneNeedsConfig += () =>
            {
                if (_Seats != null)
                    _SeatChannel.TransmitConfig(_Seats);
            };

            _SeatService.SeatConfigArrived += config =>
            {
                Dispatcher.Invoke((Action<Seat[]>)(seats =>
                {
                    _Seats = seats;
                    Canvas.ItemsSource = _Seats;
                    UpdateNames();
                }), (object)config);
            };
        }

        private Color[] _Colors = new Color[] { Colors.Red, Colors.Blue, Colors.Green, Colors.Pink, Colors.PowderBlue, Colors.SlateGray, Colors.Tan, Colors.Teal };
        private Seat[] _Seats;
        private SeatService _SeatService;
        private ServiceHost _Host;
        private ChannelFactory<ISeatServiceChannel> _ChannelFactory;
        private ISeatServiceChannel _SeatChannel;
        public void StartService()
        {
            _SeatService = new SeatService();
            _SeatService.SeatChanged += new Action<int, SeatState, SeatState>(_SeatService_SeatChanged);
            _Host = new ServiceHost(_SeatService);

            _Host.Open();

            _ChannelFactory = new DuplexChannelFactory<ISeatServiceChannel>(_SeatService, "SermoClientEndpoint");
            _SeatChannel = _ChannelFactory.CreateChannel();

            var onlineOffline = _SeatChannel.GetProperty<IOnlineStatus>();
            onlineOffline.Online += new EventHandler(onlineOffline_Online);
            onlineOffline.Offline += new EventHandler(onlineOffline_Offline);

            _SeatChannel.Init();
        }

        void onlineOffline_Offline(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => Status.Content = "Offline"));
        }

        void onlineOffline_Online(object sender, EventArgs e)
        {
            if (_Seats == null)
                _SeatChannel.NeedConfig();
            Dispatcher.BeginInvoke((Action)(() => Status.Content = "Online"));
        }

        public void UpdateNames()
        {
            if (_Seats != null)
            {
                IEnumerable<Seat> seatList = from seat in _Seats
                                                where seat.State == SeatState.Talking
                                                select seat;


                 seatList = seatList.ToList();
                 int colorIndex = 0;
                 for (int i = 0; i < seatList.Count(); i++)
                 {
                     while (seatList.Any(seat => seat.Color.HasValue && seat.Color.Value == _Colors[colorIndex]))
                         colorIndex++;
                     if (!seatList.ElementAt(i).Color.HasValue)
                     {
                         seatList.ElementAt(i).Color = _Colors[colorIndex];
                     }
                 }

                NamesListBox.ItemsSource = seatList;

            }
        }

        void _SeatService_SeatChanged(int arg1, SeatState arg2, SeatState arg3)
        {
            Dispatcher.Invoke((Action<int, SeatState, SeatState>)((seatNum, oldState, newState) =>
            {
                var seat = _Seats[seatNum];

                if (seat.State != oldState) return;

                seat.State = newState;
                UpdateNames();

            }), arg1, arg2, arg3);
        }

        public void StopService()
        {
            if (_Host != null && _Host.State != CommunicationState.Closed)
            {
                _ChannelFactory.Close();
                _Host.Close();
            }
        }

        private void ToggleSeat(Seat seat)
        {
            var oldState = seat.State;
            seat.State = (seat.State == SeatState.Sitting) ? SeatState.Talking : SeatState.Sitting;
            _SeatChannel.SeatStateChanged(IndexOf(_Seats, seat), oldState, seat.State);
        }
        private void rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleSeat((((Rectangle)sender).Tag as Seat));
            UpdateNames();
        }

        private int IndexOf<T>(T[] array, T target)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (object.ReferenceEquals(array[i], target))
                    return i;
            }
            return -1;
        }
    }
}
