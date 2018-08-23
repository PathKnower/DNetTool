using System;
using System.Collections.Generic;
using System.Text;

namespace DNet_DataContracts.Maintance
{
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
}
