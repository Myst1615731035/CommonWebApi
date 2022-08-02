using Common.Model.ApiModel;
using Common.Utils;
using Common.Utils.DBHelper;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CommonApi.Controllers.Generatore
{
    /// <summary>
    /// 获取数据库列表信息
    /// </summary>
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class GeneratorController : ControllerBase
    {
        #region 构造方法
        private readonly ISqlSugarClient _db;
        private readonly ILogger<GeneratorController> _logger;

        public GeneratorController(ISqlSugarClient db, ILogger<GeneratorController> logger)
        {
            _db = db;
            _logger = logger;
        }
        #endregion

        #region output
        /// <summary>
        /// 获取数据库列表配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<List<DBSetting>>> GetDbList()
        {
            var res =  new ContentJson<List<DBSetting>>()
            {
                success = true,
                msg = "获取成功",
            };
            if (DBConfig.MutilDb)
            {
                res.data = DBConfig.MutilDbs.allDbs.Select(t => new DBSetting() { ConfigId = t.ConfigId, DbType = t.DbType }).ToList();
            }
            else
            {
                res.data = DBConfig.MutilDbs.allDbs.Where(t=>t.ConfigId == DBConfig.MainDbConfigId).Select(t => new DBSetting() { ConfigId = t.ConfigId, DbType = t.DbType }).ToList();
            }
            return res;
        }

        /// <summary>
        /// 根据数据库Id获取数据表信息
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<List<DbTableInfo>>> GetTablesByDb(string configId)
        {
            return new ContentJson<List<DbTableInfo>>()
            {
                success = true,
                msg = "获取成功",
                data = DBHelper.db.GetConnectionScope(configId).DbMaintenance.GetTableInfoList()
            };
        }
        #endregion

        #region input
        #endregion
    }
}
