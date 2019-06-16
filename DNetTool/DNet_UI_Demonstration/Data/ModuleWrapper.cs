using DNet_DataContracts;
using DNet_DataContracts.Maintance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNet_UI_Demonstration.Data
{
    public class ModuleWrapper
    {
        public Module TargedModule { get; set; }

        public MachineLoad Load { get; set; }

        public ModuleWrapper()
        {

        }

    }
}
