using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DNet_Communication.Maintance
{
    public interface ITypeEvaluateService
    {
        /// <summary>
        /// Table of memory consumption by base structures. 
        /// Memory allocation size described in bits
        /// </summary>
        Dictionary<string, int> BaseTypeMemoryConsumptionTable { get; set; }

        /// <summary>
        /// Table of approximate memory consumption by each type. 
        /// Key - the type name.
        /// Value - approximate memory consuption.
        /// </summary>
        Dictionary<string, long> TypeMemoryConsumptionDictionary { get; set; } 

        /// <summary>
        /// Create a table of type memory consuption
        /// </summary>
        /// <returns></returns>
        Task CreateApproximateTypeMemoryConsumptionTable(params Type[] types);

    }
}
