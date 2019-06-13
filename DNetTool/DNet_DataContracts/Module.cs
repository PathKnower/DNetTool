using System;
using System.Collections.Generic;
using System.Text;

using DNet_DataContracts.Maintance;

namespace DNet_DataContracts
{
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
        public string IpAdress { get; set; }

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
