﻿using System;
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

        public ModuleHubWrapper()
        {
            TargedModule = new Module();
        }

        public ModuleHubWrapper(string id, MainHub parentHub)
        {
            ConnectionId = id;
            parentHub.UpdatedMachineInfo += ParentHub_UpdatedMachineInfo;

            TargedModule = new Module(id);
        }

        private void ParentHub_UpdatedMachineInfo(string id, MachineSpecifications info)
        {
            if (id != ConnectionId)
                return;
            
            //TODO: Rework this
            TargedModule.ModulesHostSpecs = info;
        }
    }
}