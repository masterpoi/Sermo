using System;
using System.ServiceModel;

namespace Sermo
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SeatService : ISeatService
    {
        public event Action<int, SeatState, SeatState> SeatChanged;
        public event Action<Seat[]> SeatConfigArrived;
        public event Action SomeoneNeedsConfig;

        #region ISeatService Members

        public void SeatStateChanged(int seatNumber, SeatState oldState, SeatState newState)
        {
            if (SeatChanged != null)
                SeatChanged(seatNumber, oldState, newState);
        }

        #endregion

        #region ISeatService Members


        public void NeedConfig()
        {
            if (SomeoneNeedsConfig != null)
            {
                SomeoneNeedsConfig();
            }
        }

        public void TransmitConfig(Seat[] seats)
        {
            if (SeatConfigArrived != null)
                SeatConfigArrived(seats);
        }

        #endregion

        #region ISeatService Members


        public void Init()
        {
     
        }

        #endregion
    }
}
