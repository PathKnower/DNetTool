using System;
using System.Collections.Generic;
using System.Text;

using DNet_DataContracts.Maintance;

namespace DNet_DataContracts
{
    /// <summary>
    /// Represent a module types
    /// </summary>
    //public enum ModuleTypes
    //{
    //    /// <summary>
    //    /// Main module on main connection type. Register, manage and redirect all modules between them.
    //    /// </summary>
    //    Hub,

    //    /// <summary>
    //    /// UI module, module can be write on any language, provide an ability to change UI. Reports to Controller module on 2nd connection type
    //    /// </summary>
    //    UI,

    //    /// <summary>
    //    /// Controller module, uses for accepts all requests from UI. 
    //    /// </summary>
    //    Controller,

    //    /// <summary>
    //    /// Manager module, load balancer itself
    //    /// </summary>
    //    Manager,

    //    /// <summary>
    //    /// Processing module, solve all calculation tasks.
    //    /// </summary>
    //    Processing,

    //    /// <summary>
    //    /// View module, provides all views.
    //    /// </summary>
    //    View,

    //    /// <summary>
    //    /// Add-on modules, Coming soon
    //    /// </summary>
    //    Add_on
    //}

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
        /// Module connection ID in scope of current session
        /// </summary>
        public string ConnectionID { get; set; }

        /// <summary>
        /// Remote Ip Adrress of module
        /// </summary>
        public string RemoteIpAdress { get; set; }

        /// <summary>
        /// Local Ip Adrress of module
        /// </summary>
        public string LocalIpAdress { get; set; }

        /// <summary>
        /// Module's host machine information
        /// </summary>
        public MachineSpecifications ModulesHostSpecs { get; set; }

        ///// <summary>
        ///// Available hubs (basicly here is only one hub, 
        ///// multi-hub support is coming)
        ///// </summary>
        //public List<MachineSpecifications> Hubs { get; set; } 
        //    = new List<MachineSpecifications>(); //instant initialize

        /// <summary>
        /// Module group
        /// </summary>
        public string Group { get; set; }
        
        /// <summary>
        /// Type of the current module
        /// </summary>
        public string ModuleType { get; set; }

        #region Ctor's

        public Module()
        { }

        /// <summary>
        /// Ctor's which sets id
        /// </summary>
        /// <param name="id">Connection Id</param>
        public Module(string id)
        {
            ConnectionID = id;
        }

        ///// <summary>
        ///// Ctor's which sets id and connection parent
        ///// </summary>
        ///// <param name="id">Connection Id</param>
        ///// <param name="connectionParent">Usually - Hub</param>
        //public Module(string id, MachineSpecifications connectionParent)
        //{
        //    ConnectionID = id;
        //    Hubs.Add(connectionParent);
        //}

        #endregion

        #region Events 

        //TODO:

        #endregion
    }
}
