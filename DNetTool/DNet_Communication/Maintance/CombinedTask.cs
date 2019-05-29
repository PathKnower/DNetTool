using System;
using System.Threading.Tasks;


public class CombinedTask
{

    public CombinedTask()
    {
        
    }

    public System.Threading.Tasks.Task ExecutableTask {get;set;}

    public DNet_DataContracts.Processing.Task HubTask {get;set;}

}