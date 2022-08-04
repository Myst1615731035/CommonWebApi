using Common.IService;
using Common.Model.SysModels;
using Common.Repository;
using Common.Service.Base;
using Common.Utils;
using SqlSugar;

namespace Common.Service
{
    /// <summary>
    /// SysUserServices
    /// </summary>	
    public partial class SysServices : BaseService<SysUser>, ISysUserServices
    {
        IBaseRepository<SysUser> dal;
        ISqlSugarClient db;
        public SysServices(IBaseRepository<SysUser> _dal)
        {
            dal = _dal;
            db = dal.Db;
        }

        #region ��ȡ�û���Ϣ
        /// <summary>
        /// �û���¼��ȡToken��Ϣ
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<TokenModel> GetUserInfoToken(string account, string password)
        {
            var roleIds = await GetUserQuery().Select((u, ur, r) => new
            {
                UserId = u.Id,
                Account = u.Account,
                Password = u.Password,
                RoleIds = SqlFuncExt.Stuff("SELECT ',' + Id FROM Sys_Role t WHERE t.Id = r.id"),
            }).MergeTable()
                .Where(t => t.Account == account && t.Password == password)
                .Select(t => new TokenModel() { Uid = t.UserId, Role = t.RoleIds }).FirstAsync();
            return roleIds;
        }
        /// <summary>
        /// ��ѯ�û���Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> GetUserInfo(string id)
        {
            return await GetUserQuery().Select((u, ur, r) => new
            {
                UserId = u.Id,
                u.Account,
                u.UserName,
                u.Avatar,
                u.Sex,
                u.Email,
                u.Remark,
                RoleNames = SqlFuncExt.Stuff("SELECT ',' + Name FROM Sys_Role t WHERE t.Id = r.id"),
            }).MergeTable().Where(t => t.UserId == id).FirstAsync();
        }
        /// <summary>
        /// ��ѯ�û�Ȩ����Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Permission>> GetUserAuth(string id)
        {
            //�����û����ҵ��û������н�ɫ����
            var roleIds = await GetUserQuery().Where((u, ur, r) => u.Id == id).Select((u, ur, r) => r.Id).ToListAsync();
            //���ݽ�ɫ�б��ȡ��صĲ˵��밴ťȨ������
            var permissions = await db.Queryable<Permission, RolePermissionButton>((p, rpb) => new JoinQueryInfos(JoinType.Inner, p.Id == rpb.PermissionId && !p.IsDelete))
                                        .Where((p, rpb) => roleIds.Contains(rpb.RoleId) && rpb.PermissionId != "" && rpb.PermissionId != null)
                                        .Select((p, rpb) => p)
                                        .Distinct().OrderBy(p=>p.Sort).ToListAsync();
            var buttons = await db.Queryable<Button, RolePermissionButton>((b, rpb) => new JoinQueryInfos(JoinType.Inner, b.Id == rpb.ButtonId && !b.IsDelete))
                                        .Where((b, rpb) => roleIds.Contains(rpb.RoleId) && rpb.ButtonId != "" && rpb.ButtonId != null)
                                        .Select((b, rpb) => b)
                                        .Distinct().ToListAsync();
            permissions = permissions.IsNotEmptyOrNull() && permissions.Count > 0 ? permissions : new List<Permission>();

            //����ť���ݲ��䵽�˵�������
            permissions.ForEach(t =>
            {
                t.Buttons = buttons.Where(b => b.Pid == t.Id).ToList();
            });
            //���ɲ˵����ṹ
            var list = permissions.ToTree(
                                (r, c) => { return c.Pid == ""; },
                                (r, c) => { return r.Id == c.Pid; },
                                (r, dataList) => { r.Children.AddRange(dataList); }
                                );

            return list;
        }

        /// <summary>
        /// �û���Ϣ��ѯ
        /// </summary>
        /// <returns></returns>
        private ISugarQueryable<SysUser, UserRole, Role> GetUserQuery()
        {
            return db.Queryable<SysUser, UserRole, Role>((u, ur, r) => new JoinQueryInfos(
                        JoinType.Left, u.Id == ur.UserId && !u.IsDelete,
                        JoinType.Inner, ur.RoleId == r.Id && !r.IsDelete
                    ));
        }
        #endregion


    }
}
