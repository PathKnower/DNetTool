using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Hub.Hubs;
using DNet_DataContracts.Maintance;

namespace DNet_Hub.Implements
{
    public class ModuleProcessing
    {
        public readonly string Id;

        public MachineInfo LocalMachineInfo { get; set; }

        public ModuleProcessing(string id, MainHub parentHub)
        {
            Id = id;
            parentHub.RecievedMachineInfo += ParentHub_RecievedMachineInfo;
            parentHub.UpdatedMachineInfo += ParentHub_UpdatedMachineInfo;
        }


        private void ParentHub_UpdatedMachineInfo(string id, MachineInfo info)
        {
            if (id != Id)
                return;
            
            //TODO: Rework this
            LocalMachineInfo = info;
        }

        private void ParentHub_RecievedMachineInfo(string id, MachineInfo info)
        {
            if (id != Id)
                return;

            LocalMachineInfo = info;
        }
    }
}
