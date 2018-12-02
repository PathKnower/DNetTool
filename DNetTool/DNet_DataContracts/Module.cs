using System;
using System.Collections.Generic;
using System.Text;

using DNet_DataContracts.Maintance;

namespace DNet_DataContracts
{
    /// <summary>
    /// Represent a module types
    /// </summary>
    public enum ModuleTypes
    {
        /// <summary>
        /// Main module on main connection type. Register, manage and redirect all modules between them.
        /// </summary>
        Hub,

        /// <summary>
        /// UI module, module can be write on any language, provide an ability to change UI. Reports to Controller module on 2nd connection type
        /// </summary>
        UI,

        /// <summary>
        /// Controller module, uses for accepts all requests from UI. 
        /// </summary>
        Controller,

        /// <summary>
        /// Processing module, solve all calculation tasks.
        /// </summary>
        Processing,

        /// <summary>
        /// View module, provides all views.
        /// </summary>
        View,

        /// <summary>
        /// Add-on modules, Coming soon
        /// </summary>
        Add_on
    }

    /// <summary>
    /// Represent a module which host's on machine
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Unique module ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Module's host machine information
        /// </summary>
        public MachineInfo ModuleHostInfo { get; set; }

        /// <summary>
        /// Module group
        /// </summary>
        public string Group { get; set; }
        
        /// <summary>
        /// Type of the current module
        /// </summary>
        public ModuleTypes ModuleType { get; set; }
    }
}
