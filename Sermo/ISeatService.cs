using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Sermo
{
    [ServiceContract(CallbackContract=(typeof(ISeatService)))]
    public interface ISeatService
    {
        [OperationContract(IsOneWay=true)]
        void SeatStateChanged(int seatNumber, SeatState oldState, SeatState newState);

        [OperationContract(IsOneWay = true)]
        void NeedConfig();

        [OperationContract(IsOneWay = true)]
        void TransmitConfig(Seat[] seats);

        [OperationContract(IsOneWay = true)]
        void Init();
    }

    public interface ISeatServiceChannel : ISeatService, IClientChannel
    {
    }
}
