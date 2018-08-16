using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModuleConnect.Interfaces
{
    interface IConnect
    {

         Task Connect(string moduleType);
    }
}
