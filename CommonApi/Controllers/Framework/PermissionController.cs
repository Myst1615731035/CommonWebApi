using Common.IService.ISysServices;
using Common.Model.ApiModel;
using Common.Model.SysModels;
using Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CommonApi.Controllers
{
    /// <summary>
    /// SysController
    /// </summary>	
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PermissionController : ControllerBase
    {
        #region 接口构造
        private readonly IPermissionServices _service;
        private readonly IUser _user;
        private readonly ILogger<PermissionController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PermissionController(IPermissionServices service, IUser user, ILogger<PermissionController> logger)
        {
            _service = service;
            _user = user;
            _logger = logger;
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<Pagination>> GetList([FromBody] Pagination pagination)
        {
            var exp = Expressionable.Create<Permission>();
            // 增加查询条件

            var list = await _service.QueryPage(exp.ToExpression(), pagination);
            return new ContentJson<Pagination>()
            {
                msg = "success",
                success = true,
                data = list
            };
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<object>> GetEntity([FromBody] object id)
        {
            var exp = Expressionable.Create<Permission>();
            // 增加查询条件

            var entity = await _service.QueryById(id);
            return new ContentJson<object>()
            {
                msg = "success",
                success = true,
                data = entity
            };
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<string>> SaveEntity([FromBody] Permission entity)
        {
            //结果定义
            var result = new ContentJson<string>()
            {
                msg = "操作失败",
                success = false,
                data = ""
            };

            // Update
            if (entity.Id.IsNotEmptyOrNull())
            {
                if (await _service.Update(entity))
                {
                    result.msg = "更新成功";
                    result.success = true;
                }

            }
            // Insert
            else
            {
                entity.CreateId = _user.ID;
                entity.CreateName = _user.Name;
                if (await _service.Add(entity) > 0)
                {
                    result.msg = "添加成功";
                    result.success = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<string>> Delete([FromBody] Permission entity)
        {
            //结果定义
            var result = new ContentJson<string>()
            {
                msg = "操作失败",
                success = false,
                data = ""
            };

            // Delete
            if (entity.Id.IsNotEmptyOrNull())
            {
                if (await _service.Delete(entity))
                {
                    result.msg = "数据已删除";
                    result.success = true;
                }

            }
            return result;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ContentJson<string>> DeleteById([FromBody] object Id)
        {
            //结果定义
            var result = new ContentJson<string>()
            {
                msg = "操作失败",
                success = false,
                data = ""
            };

            // Delete
            if (Id.IsNotEmptyOrNull())
            {
                if (await _service.DeleteById(Id))
                {
                    result.msg = "数据已删除";
                    result.success = true;
                }
            }
            return result;
        }
        #endregion
    }
}
