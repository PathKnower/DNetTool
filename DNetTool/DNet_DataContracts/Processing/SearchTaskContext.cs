using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;

namespace DNet_DataContracts.Processing
{
    public class SearchTaskContext : TaskContext
    {
        /// <summary>
        /// Approximate area of search, help to detect type
        /// </summary>
        public string SearchArea { get; set; }

        /// <summary>
        /// Search filer. Use System.Linq.Dynamic.Core for parse
        /// </summary>
        public string SearchExpression { get; set; }

        /// <summary>
        /// Target amount of requred results
        /// </summary>
        public int ResultAmount { get; set; }

        /// <summary>
        /// If required to take last
        /// </summary>
        public bool TakeLast { get; set; }

        /*
         * const string exp = @"(Person.Age > 3 AND Person.Weight > 50) OR Person.Age < 3";
         * var p = Expression.Parameter(typeof(Person), "Person"); //Person - class
         * var e = System.Linq.Dynamic.DynamicExpression.ParseLambda(new[] { p }, null, exp);
         * 
         * 
         */
    }
}
