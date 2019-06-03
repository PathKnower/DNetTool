using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Reflection;

using DNet_Communication.Maintance;
using DNet_UI_Demonstration.Data;

namespace DNet_UI_Demonstration.Logic
{
    public class UITypeEvaluateService : BaseTypeEvaluateService, IUITypeEvaluateService
    {
        //all in bits
        const short MemoryConsumptionForEmptyStruct = 8;
        const short MemoryConsumptionForEmptyClass = 8;  //

        const short MemoryConsumptionByRefernceForClassIn32BitSystem = 32; //for x64 should x2

        public UITypeEvaluateService()
            : base()
        {

        }

        public override async Task CreateApproximateTypeMemoryConsumptionTable(params Type[] types)
        {

            foreach(Type type in types)
            {
                IEnumerable<FieldInfo> allFieldFromType = type.GetRuntimeFields();

                long aproximateMemoryConsumption = 0;

                int clusterSize = (Environment.Is64BitOperatingSystem ? MemoryConsumptionByRefernceForClassIn32BitSystem * 2 : MemoryConsumptionByRefernceForClassIn32BitSystem);

                if (type.IsClass)
                {
                    aproximateMemoryConsumption = MemoryConsumptionForEmptyClass;
                    aproximateMemoryConsumption += clusterSize;
                }
                else
                    aproximateMemoryConsumption = MemoryConsumptionForEmptyStruct;

                foreach(FieldInfo field in allFieldFromType)
                {
                    if(BaseTypeMemoryConsumptionTable.TryGetValue(field.FieldType.ToString(), out int memoryConsumptionInBits))
                    {
                        aproximateMemoryConsumption += memoryConsumptionInBits; 
                    }
                    else
                    {
                        aproximateMemoryConsumption += clusterSize; //mean that this fiels - is class
                    }
                }

                if (aproximateMemoryConsumption % clusterSize != 0) // equality to stack size 
                {
                    aproximateMemoryConsumption += Math.Abs(clusterSize - (aproximateMemoryConsumption % clusterSize));
                }

                TypeMemoryConsumptionDictionary.Add(type.Name, aproximateMemoryConsumption);
            }
        }

        public async Task Initialize()
        {
            await CreateApproximateTypeMemoryConsumptionTable(typeof(User), typeof(News));
        }
    }
}
