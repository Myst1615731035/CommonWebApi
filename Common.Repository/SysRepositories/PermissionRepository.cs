using Common.Model.SysModels;
using Common.IRepository;

namespace Common.Repository
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
