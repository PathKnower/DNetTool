using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Hub.Hubs;
using DNet_DataContracts.Maintance;
using DNet_DataContracts;

namespace DNet_Hub.Communication
{
    public class ModuleHubWrapper
    {
        public string ConnectionId { get; set; }

        public Module TargedModule { get; set; }

        public MachineLoad Load { get; set; }

        public ModuleHubWrapper()
        {
            TargedModule = new Module();
            Load = new MachineLoad();
        }

        public ModuleHubWrapper(string id, MainHub parentHub)
        {
            ConnectionId = id;
            MainHub.UpdatedMachineInfo += ParentHub_UpdatedMachineInfo;
            MainHub.UpdateMachineLoad += ParentHub_UpdateMachineLoad;

            TargedModule = new Module(id);
            Load = new MachineLoad();
        }

        private void ParentHub_UpdateMachineLoad(string id, MachineLoad info)
        {
            if (id != ConnectionId)
                    return;

            Load = info;
        }

        private void ParentHub_UpdatedMachineInfo(string id, MachineSpecifications info)
        {
            if (id != ConnectionId)
                return;
            
            
            TargedModule.ModulesHostSpecs = info;
        }
    }
}
