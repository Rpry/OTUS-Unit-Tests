using IT2.Trading.StateMachine.Common;
using IT2.Trading.StateMachine.Contracts.Enums;
using IT2.Trading.StateMachine.DataAccess.Context;
using IT2.Trading.StateMachine.DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using IT2.Trading.StateMachine.DataAccess.Models.Fields;

namespace IT2.Trading.StateMachine.Service
{
    /// <summary>
    /// Общий класс для сидинга енумов
    /// </summary>
    public static class SeedAllEnums
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            SeedEnum<FieldDataType, FieldType>(serviceProvider);
            SeedEnum<ValidationRuleCode, ValidationRule>(serviceProvider);
        }

        private static void SeedEnum<TEnum, TEntity>(IServiceProvider serviceProvider)
            where TEnum : struct, IConvertible
            where TEntity : BaseEnumEntity<TEnum>, new()
        {
            using (IServiceScope scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                TradingContext context = scope.ServiceProvider.GetService<TradingContext>();
                bool isAnyAdded = false;

                Dictionary<int, string> allEnumValues = EnumHelper.AllEnumValues(typeof(TEnum));

                foreach (KeyValuePair<int, string> kvp in allEnumValues)
                {
                    bool added = AddFieldType<TEntity, TEnum>(context, kvp.Key, kvp.Value);
                    isAnyAdded = isAnyAdded || added;
                }

                if (isAnyAdded)
                {
                    context.SaveChanges();
                }
            }
        }

        private static bool AddFieldType<TEntity, TEnum>(TradingContext context, object id, string fieldDescription)
            where TEnum : struct, IConvertible
            where TEntity : BaseEnumEntity<TEnum>, new()
        {
            TEntity fieldTypeFromDb = context.Find<TEntity>((TEnum)id);
            if (fieldTypeFromDb != null)
            {
                return false;
            }

            TEntity entity = new TEntity { Code = (TEnum)id, Description = fieldDescription };
            context.Add(entity);
            return true;
        }
    }
}
