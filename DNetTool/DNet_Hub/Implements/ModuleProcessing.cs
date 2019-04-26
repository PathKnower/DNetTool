using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Hub.Hubs;
using DNet_DataContracts.Maintance;

namespace DNet_Hub.Implements
{
    public class ModuleInfo
    {
        public readonly string ConnectionId;

        public string RemoteUri { get; set; }
        public string LocalUri { get; set; }

        public MachineSpecifications TargetMachineSpecifications { get; set; }

        public ModuleInfo()
        {

        }

        public ModuleInfo(string id, MainHub parentHub)
        {
            ConnectionId = id;
            parentHub.RecievedMachineInfo += ParentHub_RecievedMachineInfo;
            parentHub.UpdatedMachineInfo += ParentHub_UpdatedMachineInfo;
        }


        private void ParentHub_UpdatedMachineInfo(string id, MachineSpecifications info)
        {
            if (id != ConnectionId)
                return;
            
            //TODO: Rework this
            TargetMachineSpecifications = info;
        }

        private void ParentHub_RecievedMachineInfo(string id, MachineSpecifications info)
        {
            if (id != ConnectionId)
                return;

            TargetMachineSpecifications = info;
        }
    }
}
