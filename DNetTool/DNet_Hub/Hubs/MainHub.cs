﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_DataContracts.Maintance;
using DNet_DataContracts;

using DNet_Hub.Communication;
using Microsoft.Extensions.Logging;

using DNet_Hub.Processing;

namespace DNet_Hub.Hubs
{
    public class MainHub : Hub
    {
        private static string HubGUID = null;

        public delegate void MachineHardwareEvent(string id, MachineSpecifications info);
        public delegate void MachineLoadEvent(string id, MachineLoad info);
        public static event MachineHardwareEvent UpdatedMachineInfo;
        public static event MachineLoadEvent UpdateMachineLoad;

        private ILogger<MainHub> _logger;
        private ITaskHandlerService _taskHandlerService;
        private ILoadBalancerService _loadBalancerService;


        public MainHub(ILogger<MainHub> logger, ITaskHandlerService taskHandlerService, ILoadBalancerService loadBalancerService)
        {
            _logger = logger;
            _taskHandlerService = taskHandlerService;
            _loadBalancerService = loadBalancerService;

            if(string.IsNullOrEmpty(HubGUID))
            {
                HubGUID = Guid.NewGuid().ToString();
                _logger.LogInformation($"Guid of current Hub for this session: \'{HubGUID}\'");
            }

            if (UserGroup == null)
                UserGroup = new Dictionary<string, string>();

            if (Modules == null)
                Modules = new List<ModuleHubWrapper>();

            if (TaskHistory == null)
                TaskHistory = new List<DNet_DataContracts.Processing.Task>();
        }

        #region Properties

        /// <summary>
        /// Store module connection id (key) and module group (value)
        /// </summary>
        public static Dictionary<string, string> UserGroup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static List<ModuleHubWrapper> Modules { get; set; } 


        private static List<DNet_DataContracts.Processing.Task> TaskHistory { get; set; }

        #endregion

        #region Register Logic

        /// <summary>
        /// Register any module on their group
        /// </summary>
        /// <param name="groupName">Module group</param>
        /// <returns></returns>
        public async Task RegisterModule(string moduleType)
        {
            ModuleHubWrapper newModule = new ModuleHubWrapper(Context.ConnectionId, this);
            newModule.TargedModule.ModuleType = moduleType;
            Modules.Add(newModule); //add new module to maintance module info

            await this.Groups.AddToGroupAsync(Context.ConnectionId, moduleType);
            UserGroup.Add(Context.ConnectionId, moduleType);

            _logger.LogInformation($"Successfully registered module: {moduleType}, Connection ID: {Context.ConnectionId}");

            await this.Clients.Caller.SendAsync("OnRegister", "Ok", HubGUID); //Callback that successfull register module
            await this.Clients.Caller.SendAsync("CollectMachineInfo");
        }
        
        /// <summary>
        /// Unregister module from group
        /// </summary>
        /// <returns></returns>
        public async Task UnregisterModule(string connectionID)
        {
            string connectionId = connectionID;
            await Groups.RemoveFromGroupAsync(connectionId, UserGroup[connectionId]);

            _logger.LogInformation($"Unregistering module: {UserGroup[connectionId]}, Connection ID: {connectionId}");
            UserGroup.Remove(connectionId);

            var module = Modules.FirstOrDefault(x => x.ConnectionId == connectionId);
            if(module != null)
                Modules.Remove(module);

            //TODO: If there is a task is on run, cancel it
        }

        /// <summary>
        /// Handle module disconnect and remove it from group
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            UnregisterModule(Context.ConnectionId).Wait();
            _logger.LogInformation($"Module disconnected: ConnectionId: \'{Context.ConnectionId}\'");
            //Console.WriteLine($"{Context.ConnectionId} disconnected");
            return base.OnDisconnectedAsync(exception);
        }

        #endregion


        #region Task Handling

        public async Task TaskReciever(DNet_DataContracts.Processing.Task task)
        {
            task.RequestedBy = Modules.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId)?.TargedModule;

            await _loadBalancerService.SelectTargetModule(task, this);
        }

        public async Task SendTask(string connectionId, DNet_DataContracts.Processing.Task task)
        {
            task.IsExecuting = true;
            TaskHistory.Add(task);
            await this.Clients.Client(connectionId).SendAsync("TaskRecieve", task);
        }

        public async Task TaskResult(DNet_DataContracts.Processing.Task task)
        {
            _logger.LogDebug($"Task with ID: \'{task.Id}\' successfully finished");
            var temp = TaskHistory.FirstOrDefault(x => x.Id == task.Id);
            TaskHistory.Remove(temp);
            TaskHistory.Add(task);
            await this.Clients.Client(task.RequestedBy.ConnectionID).SendAsync("ResultRecieve", task);
        }

        public async Task ModuleUnsupportTaskType(DNet_DataContracts.Processing.Task task)
        {
            //Handle this 
        }

        #endregion


        #region Maintance region

        /// <summary>
        /// Machine info handler
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <returns></returns>
        public async Task RecieveMachineInfo(MachineSpecifications moduleInfo)
        {
            UpdatedMachineInfo?.Invoke(Context.ConnectionId, moduleInfo);
        }

        public async Task RecieveModuleActivity(MachineLoad machineLoad)
        {
            //_logger.LogDebug($"Update machine load on machine {Context.ConnectionId} \n CPU Load: {machineLoad.CPULoad}% , Memory Load {machineLoad.MemoryLoad}%");
            UpdateMachineLoad?.Invoke(Context.ConnectionId, machineLoad);
        }

        #endregion

        #region Helpers

        public async Task RequestLoadUpdate(IEnumerable<ModuleHubWrapper> modules)
        {
            await this.Clients.Clients(modules.Select(x => x.ConnectionId).
                ToList()).SendAsync("GetMachineLoad"); //Collect all load from potential modules
        }


        public async Task ShareModuleInfo()
        {
            await this.Clients.Caller.SendAsync("RecieveModuleInfo", Modules.ToArray());
        }


        public async Task ShareTaskInfo()
        {
            await this.Clients.Caller.SendAsync("RecieveTaskInfo", TaskHistory.ToArray());
        }
        //TODO: Set IP adress to module

        #endregion
    }
}
