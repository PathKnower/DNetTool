using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DNet_DataContracts;

namespace DNet_Communication.Connection
{
    public delegate void ConnectionHandler(string HubGUID); 

    public interface IConnectionService : IDisposable
    {
        event ConnectionHandler SuccessfullRegister;

        void ScheduleConnectionInitialize(TimeSpan interval, string moduleType);


    }
}