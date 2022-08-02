using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Utils.DBHelper
{
    public class SplitTableService: ISplitTableService
    {
        /// <summary>
        /// 获取所有分表
        /// </summary>
        /// <param name="db"></param>
        /// <param name="EntityInfo"></param>
        /// <param name="tableInfos"></param>
        /// <returns></returns>
        public List<SplitTableInfo> GetAllTables(ISqlSugarClient db, EntityInfo EntityInfo, List<DbTableInfo> tableInfos)
        {
            List<SplitTableInfo> list = new List<SplitTableInfo>();
            if (DBConfig.SplitTables.Count(t=>t == EntityInfo.EntityName) > 0)
            {
                var key = DBConfig.SplitTables.Find(n => n == EntityInfo.EntityName);
                var tables = tableInfos.Where(t => t.Name.Contains(key)).ToList();
                if (tables.Count() > 0)
                {
                    tables.ForEach(item =>
                    {
                        list.Add(new SplitTableInfo() { TableName = item.Name });
                    });
                }
            }
            return list.OrderBy(t => t.TableName).ToList();
        }
        public object GetFieldValue(ISqlSugarClient db, EntityInfo entityInfo, SplitType splitType, object entityValue)
        {
            var splitColumn = entityInfo.Columns.FirstOrDefault(it => it.PropertyInfo.GetCustomAttribute<SplitFieldAttribute>() != null);
            var value = splitColumn.PropertyInfo.GetValue(entityValue, null);
            return value;
        }

        public string GetTableName(ISqlSugarClient db, EntityInfo EntityInfo)
        {
            return EntityInfo.DbTableName;
        }
        public string GetTableName(ISqlSugarClient db, EntityInfo EntityInfo, SplitType type)
        {
            return EntityInfo.DbTableName;
        }
        public string GetTableName(ISqlSugarClient db, EntityInfo entityInfo, SplitType splitType, object fieldValue)
        {
            return fieldValue.IsEmpty() ? entityInfo.DbTableName: $"{fieldValue}_{entityInfo.DbTableName}"; 
        }
    }
}
