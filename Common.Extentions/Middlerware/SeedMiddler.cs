using Common.Utils;
using Common.Utils.DBHelper;
using log4net;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using SqlSugar;
using System.Reflection;

namespace Common.Extentions.Middlerware
{
    public static class SeedMiddler
    {
        private static ILog log = LogManager.GetLogger(typeof(SeedMiddler));
        private static SqlSugarScope dbs = DBHelper.db;
        public static void UseSeedMiddler(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if(dbs == null) throw new ArgumentNullException(nameof(dbs));

            if(AppConfig.Get("Automation", "InitDataTable").ObjToBool())
            {
                Task.Run(InitSeed).Wait();
            }
        }


        public static void InitSeed()
        {
            #region 获取所有实体类
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = Directory.GetFiles(path, "Common.Model.dll").Select(Assembly.LoadFrom).ToArray();

            // 项目框架实体类
            var SysEntities = referencedAssemblies.SelectMany(a => a.DefinedTypes).Select(type => type.AsType())
                            .Where(x => x.IsClass && x.Namespace != null && x.Namespace.Equals("Common.Model.SysModels")).ToList();

            // 业务实体类
            var WorkEntities = referencedAssemblies.SelectMany(a => a.DefinedTypes).Select(type => type.AsType())
                            .Where(x => x.IsClass && x.Namespace != null && x.Namespace.Equals("Common.Model.Entities")).ToList();
            #endregion

            #region 循环创建所有库与表
            DBConfig.MutilDbs.allDbs.ForEach(t =>
            {
                "Create DataBase: ".WriteInfoLine();
                string log = $"{t.ConfigId}({t.DbTypeName}):  {t.ConnectionString}; ";
                #region Oracle处理
                //Oracle 数据库不支持该操作
                if (t.DbType == DbType.Oracle)
                {
                    log += $"  {false}\r\n Error: Oracle 数据库不支持该操作，可手动创建Oracle数据库!";
                    log.WriteErrorLine();
                    return;
                }
                #endregion

                //创建数据库
                ISqlSugarClient db = dbs.GetConnectionScope(t.ConfigId);
                log += $"  {db.DbMaintenance.CreateDatabase()}";
                log.WriteSuccessLine();

                "添加或更新项目框架实体类表".WriteInfoLine();
                SysEntities.ForEach(e =>
                {
                    var SugarTableAttr = e.GetCustomAttribute<SugarTable>();
                    var SplitTableAttr = e.GetCustomAttribute<SplitTableAttribute>();
                    #region 更新分表
                    if (SplitTableAttr.IsNotEmptyOrNull())
                    {
                        db.CodeFirst.SplitTables().InitTables(e);
                        ConsoleHelper.WriteSuccessLine($"{e.Name}({SugarTableAttr.TableName}) split table created or update successfully!");
                        Console.WriteLine();
                    }
                    #endregion

                    // 分表配置的实体类，是否要生成源表
                    if (DBConfig.OriginSplitTableInit && SplitTableAttr.IsNotEmptyOrNull())
                    {
                        db.CodeFirst.InitTables(e);
                        $"{e.Name}({SugarTableAttr.TableName})  created or update successfully!".WriteSuccessLine();
                    }
                    else
                    {
                        db.CodeFirst.InitTables(e);
                        $"{e.Name}({SugarTableAttr.TableName})  created or update successfully!".WriteSuccessLine();
                    }
                });

                WorkEntities.ForEach(w =>
                {
                    JsonConvert.DeserializeAnonymousType("", new { name = "", sex = 0 });
                    JsonConvert.DeserializeObject<dynamic>("");
                });
            });
            #endregion
        }
    }
}
