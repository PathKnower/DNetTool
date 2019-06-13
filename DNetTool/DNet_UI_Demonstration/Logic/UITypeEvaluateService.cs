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

        public override async Task CreateApproximateTypeMemoryConsumptionDictionary(params Type[] types)
        {
            foreach(Type type in types)
            {
                if (TypeMemoryConsumptionDictionary.ContainsKey(type.Name))
                    continue;

                IEnumerable<FieldInfo> allFieldFromType = type.GetRuntimeFields();

                long aproximateMemoryConsumption = 0;

                //вычисление размера блока памяти
                int clusterSize = (Environment.Is64BitOperatingSystem ? MemoryConsumptionByRefernceForClassIn32BitSystem * 2 : MemoryConsumptionByRefernceForClassIn32BitSystem);

                if (type.IsClass)
                {
                    //прибавляем выделенное пространство под класс
                    aproximateMemoryConsumption = MemoryConsumptionForEmptyClass;
                    // прибавляем вес ссылки в битах (По умолчанию равен размеру блока памяти)
                    aproximateMemoryConsumption += clusterSize; 
                }
                else
                    aproximateMemoryConsumption = MemoryConsumptionForEmptyStruct;

                // Проход по всем полям
                foreach (FieldInfo field in allFieldFromType) 
                {
                    //Если сущность поля базовая
                    if(BaseTypeMemoryConsumptionTable.TryGetValue(field.FieldType.ToString(), out int memoryConsumptionInBits))
                    { // то прибавляем необходимое значение
                        aproximateMemoryConsumption += memoryConsumptionInBits; 
                    }
                    else
                    { //иначе оцениваем сущность поля
                        // Ждём оценки сущности поля
                        //await CreateApproximateTypeMemoryConsumptionDictionary(field.FieldType);
                        //если оценка прошла успешно
                        if (TypeMemoryConsumptionDictionary.TryGetValue(field.FieldType.ToString(), out long fieldTypeMemoryConsmption))
                            //то прибавляем оценку сущности поля к оценке текущей сущности
                            aproximateMemoryConsumption += fieldTypeMemoryConsmption;
                        else
                            //иначе прибавляем размер ссылки
                            aproximateMemoryConsumption += clusterSize;
                    }
                }
                // Выравниваем по размеру блока
                if (aproximateMemoryConsumption % clusterSize != 0) 
                {
                    aproximateMemoryConsumption += Math.Abs(clusterSize - (aproximateMemoryConsumption % clusterSize));
                }
                //добавляем в словарь
                TypeMemoryConsumptionDictionary.Add(type.Name, aproximateMemoryConsumption); 
            }
        }

        public async Task Initialize()
        {
            await CreateApproximateTypeMemoryConsumptionDictionary(typeof(User), typeof(News));
        }
    }
}
