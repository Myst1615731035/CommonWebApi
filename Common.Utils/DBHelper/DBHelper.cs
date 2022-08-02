using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils.DBHelper
{
    public class DBHelper
    {
        public static readonly SqlSugarScope db = GetSqlSugarScope();

        private static SqlSugarScope GetSqlSugarScope()
        {
            // 数据库链接配置
            var list = new List<ConnectionConfig>();

            #region 从库配置
            var slaveList = new List<SlaveConnectionConfig>();
            DBConfig.MutilDbs.slaveDbs.ForEach(s =>
            {
                slaveList.Add(new SlaveConnectionConfig()
                {
                    HitRate = s.HitRate,
                    ConnectionString = s.ConnectionString
                });
            });
            #endregion

            #region 所有库配置
            DBConfig.MutilDbs.allDbs.ForEach(t =>
            {
                list.Add(new ConnectionConfig()
                {
                    ConfigId = t.ConfigId,
                    ConnectionString = t.ConnectionString,
                    DbType = t.DbType,
                    IsAutoCloseConnection = true,
                    SlaveConnectionConfigs = slaveList,
                    InitKeyType = InitKeyType.Attribute,

                    #region AOP 日志
                    AopEvents = new AopEvents()
                    {
                        #region 数据过滤
                        DataExecuting = (oldValue, entityInfo) => { },
                        #endregion

                        #region sql，param过滤器
                        OnExecutingChangeSql = (sql, paras) =>
                        {
                            return new KeyValuePair<string, SugarParameter[]>(sql, paras);
                        },
                        #endregion

                        #region SQL执行前
                        OnLogExecuting = (sql, paras) => 
                        {
                            LogHelper.Info(sql);
                        },
                        #endregion

                        #region SQL执行后
                        OnLogExecuted = (sql, paras) => { },
                        #endregion

                        #region 差异日志
                        OnDiffLogEvent = d => { },
                        #endregion

                        #region 错误日志
                        OnError = err => 
                        {
                            LogHelper.Error(err);
                        },
                        #endregion
                    },
                    #endregion

                    #region MoreSettings
                    MoreSettings = new ConnMoreSettings()
                    {
                        //IsWithNoLockQuery = true,
                        IsAutoRemoveDataCache = true
                    },
                    #endregion

                    #region ConfigureExternalServices
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityNameService = (type, entityInfo) => { },
                        EntityService = (prop, entityInfo) => 
                        { 
                            if(prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                entityInfo.IsNullable = true;
                            }
                        },
                        SqlFuncServices = SqlFuncExt.SqlFuncExtList(),
                        SplitTableService = new SplitTableService()
                    }
                    #endregion
                });
            });
            #endregion

            return new SqlSugarScope(list);
        }
    }
}
