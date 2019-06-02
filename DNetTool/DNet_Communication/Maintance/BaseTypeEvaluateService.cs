using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DNet_Communication.Maintance
{
    public class BaseTypeEvaluateService : ITypeEvaluateService
    {
        /// <summary>
        /// .Net base types array
        /// </summary>
        public string[] BaseTypesList { get; set; } = new string[]
        {
            "bool", "System.Boolean",
            "byte", "System.Byte",
            "sbyte", "System.SByte",
            "char", "System.Char",
            "short", "System.Int16",
            "ushort", "System.UInt16",
            "int", "System.Int32",
            "uint", "System.UInt32",
            "float", "System.Single",
            "long", "System.Int64",
            "ulong", "System.UInt64",
            "double", "System.Double",
            "decimal", "System.Decimal",
        };

        public Dictionary<string, int> BaseTypeMemoryConsumptionTable { get; set; } = new Dictionary<string, int>()
        {
            { "bool", 1 }, { "System.Boolean", 1 },
            { "byte", 8 }, { "System.Byte", 8 },
            { "sbyte", 8 }, { "System.SByte", 8 },
            { "char", 16 }, { "System.Char", 16 },
            { "short", 16 }, { "System.Int16", 16 },
            { "ushort", 16 }, { "System.UInt16", 16 },
            { "int", 32 }, { "System.Int32", 32 },
            { "uint", 32 }, { "System.UInt32", 32 },
            { "float", 32 }, { "System.Single", 32 },
            { "long", 64 }, { "System.Int64", 64 },
            { "ulong", 64 }, { "System.UInt64", 64 },
            { "double", 64 }, { "System.Double", 64 },
            { "decimal", 128 }, { "System.Decimal", 128 }
        };


        public Dictionary<string, long> TypeMemoryConsumptionDictionary { get; set; }

        public virtual async Task CreateApproximateTypeMemoryConsumptionTable(params Type[] types)
        {

        }
    }
}
