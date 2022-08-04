using Common.Model.SysModels;
using Common.IRepository;

namespace Common.Repository
{
    /// <summary>
    /// ButtonRepository
    /// </summary>	
    public partial class ButtonRepository : BaseRepository<Button>, IButtonRepository
    {
        public ButtonRepository() : base()
        {
        }
    }
}
