using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Linq.Dynamic.Core;

using DNet_Communication.Maintance;
using DNet_Communication.Connection;

using DNet_DataContracts.Processing;
using System.Linq.Expressions;
using DNet_PostgreSQL_Demonstration.DataContract;

namespace DNet_PostgreSQL_Demonstration.Processing
{
    public class PostgreSQLDemonstrationTaskHandlerService : BaseTaskHandlerService, IPostgreSQLDemonstrationTaskHandlerService, IDisposable
    {
        ApplicationContext _context;

        public PostgreSQLDemonstrationTaskHandlerService(IConnect connectionInstance, ApplicationContext context)
            : base(connectionInstance)
        {
            _context = context;
           
        }

        public void Initialize()
        {
            _connectionInstance.TaskRecieve += PostgreSQLDemonstrationTaskHandlerService_TaskRecieve;
        }

        private void _connectionInstance_TaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            //throw new NotImplementedException();
            Console.WriteLine("PSQL 11 Service: " + task.Id);
        }

        private async void PostgreSQLDemonstrationTaskHandlerService_TaskRecieve(DNet_DataContracts.Processing.Task task)
        {
            Console.WriteLine("PSQL Service: " + task.Id);

            if(!task.ModuleType.Contains(_connectionInstance.ModuleType) || task.Type != DNet_DataContracts.Processing.TaskType.Search)
            {
                await _connectionInstance.SendToHub("ModuleUnsupportTaskType", task);
                return;
            }

            SearchTaskContext context = task.SearchContext;

            Type type = null;
            IQueryable queryable;

            switch(context.SearchArea)
            {
                case "user":
                    type = typeof(User);
                    //queryable = _context.Users.Where(context.SearchExpression);
                    break;

                case "news":
                    type = typeof(News);
                    break;

                default:
                    break;
            }

            if(type == null)
            {
                await _connectionInstance.SendToHub("ModuleUnsupportTaskType", task);
                return;
            }

            task.IsFinished = true;
            task.SearchContext.Result = "sample";

            await _connectionInstance.SendToHub("TaskResult", task);

            //ParameterExpression usedType = Expression.Parameter(type, context.SearchAlias);

            //LambdaExpression e = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { usedType }, null, context.SearchExpression);
            
        }

        

        public void Dispose()
        {
            _connectionInstance.TaskRecieve -= PostgreSQLDemonstrationTaskHandlerService_TaskRecieve;
            _context.Dispose();
            
        }
    }
}
