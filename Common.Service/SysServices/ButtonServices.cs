using Common.IServices;
using Common.Model.SysModels;
using Common.Repository;
using Common.Service.Base;
using SqlSugar;

namespace Common.Service
{
    /// <summary>
    /// ButtonServices
    /// </summary>	
    public partial class ButtonServices : BaseService<Button>, IButtonServices
    {
        IBaseRepository<Button> dal;
        ISqlSugarClient db;
        public ButtonServices(IBaseRepository<Button> _dal)
        {
            dal = _dal;
            db = dal.Db;
        }
    }
}
