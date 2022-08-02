using Common.Model.SysModels;
using Common.Repository.ISysRepositories;

namespace Common.Repository.SysRepositories
{
    /// <summary>
    /// PermissionRepository
    /// </summary>	
    public partial class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository() : base()
        {
        }
    }
}
