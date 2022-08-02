using Common.Model.Base;
using SqlSugar;

namespace Common.Model.SysModels
{
    ///<summary>
    /// Role
    ///</summary>
    [SugarTable("Sys_Role")]
    public partial class Role : RootEntity<string>
    {
        [SugarColumn(IsNullable = false, ColumnDescription = "角色名称", ColumnDataType = "varchar", Length = 50)]
        public string Name { get; set; }


        [SugarColumn(IsNullable = false, ColumnDescription = "描述", ColumnDataType = "varchar", Length = 500)]
        public string Description { get; set; }
    }
}
