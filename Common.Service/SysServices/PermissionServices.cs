using Common.IService.ISysServices;
using Common.Model.SysModels;
using Common.Repository;
using Common.Service.Base;
using SqlSugar;

namespace Common.Service.SysServices
{
    /// <summary>
    /// PermissionServices
    /// </summary>	
    public partial class PermissionServices : BaseService<Permission>, IPermissionServices
    {
        IBaseRepository<Permission> dal;
        ISqlSugarClient db;
        public PermissionServices(IBaseRepository<Permission> _dal)
        {
            dal = _dal;
            db = dal.Db;
        }
    }
}
