using Common.Model.Base;
using SqlSugar;

namespace Common.Model.SysModels
{
    ///<summary>
    /// Button
    ///</summary>
    [SugarTable("Sys_Interface")]
    public partial class Interface : RootEntity<string>
    {
        [SugarColumn(IsNullable = false, ColumnDescription = "访问路径", ColumnDataType = "varchar", Length = 50)]
        public string Url { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "描述", ColumnDataType = "varchar", Length = 500)]
        public string Description { get; set; }
    }
}
