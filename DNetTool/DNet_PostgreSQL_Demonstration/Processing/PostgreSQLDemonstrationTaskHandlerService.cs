using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DNet_Communication.Maintance;
using DNet_Communication.Connection;

namespace DNet_PostgreSQL_Demonstration.Processing
{
    public class PostgreSQLDemonstrationTaskHandlerService : BaseTaskHandlerService, IPostgreSQLDemonstrationTaskHandlerService
    {

        public PostgreSQLDemonstrationTaskHandlerService(IConnect connectionService)
            : base(connectionService)
        {
            TaskRecieve += PostgreSQLDemonstrationTaskHandlerService_TaskRecieve;
        }

        private void PostgreSQLDemonstrationTaskHandlerService_TaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            Console.WriteLine("PSQL Service: " + task.Id);
        }


    }
}
