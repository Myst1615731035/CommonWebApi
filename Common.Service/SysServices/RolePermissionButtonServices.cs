using Common.IServices;
using Common.Model.ApiModel;
using Common.Model.SysModels;
using Common.Repository;
using Common.Service.Base;
using SqlSugar;

namespace Common.Service
{
    /// <summary>
    /// RolePermissionButtonServices
    /// </summary>	
    public partial class RolePermissionButtonServices : BaseService<RolePermissionButton>, IRolePermissionButtonServices
    {
        IBaseRepository<RolePermissionButton> dal;
        ISqlSugarClient db;
        public RolePermissionButtonServices(IBaseRepository<RolePermissionButton> _dal)
        {
            dal = _dal;
            db = dal.Db;
        }

        public async Task<List<PermissionItem>> GetPermissionItem()
        {
            var list = await db.Queryable<Role, RolePermissionButton, Permission, Button, Interface>((r, rpb, p, b, i) => new JoinQueryInfos(
                    JoinType.Left, r.Id == rpb.RoleId,
                    JoinType.Inner, rpb.PermissionId == p.Id && rpb.PermissionId != null && rpb.PermissionId != "",
                    JoinType.Inner, rpb.ButtonId == b.Id && rpb.ButtonId != null && rpb.ButtonId != "",
                    JoinType.Left, p.Fid == i.Id && p.Fid != null && p.Fid != "",
                    JoinType.Left, b.Fid == i.Id && b.Fid != null && b.Fid != ""
                )).Where((r, rpb, p, b, i) => !r.IsDelete && !rpb.IsDelete && !p.IsDelete && !b.IsDelete && !i.IsDelete)
                .Select((r, rpb, p, b, i) => new PermissionItem()
                {
                    Role = r.Id,
                    Url = i.Url
                }).ToListAsync();
            return list;
        }

    }
}
