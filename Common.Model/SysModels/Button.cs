using Common.Model.Base;
using SqlSugar;

namespace Common.Model.SysModels
{
    ///<summary>
    /// Button
    ///</summary>
    [SugarTable("Sys_Button")]
    public partial class Button : RootEntity<string>
    {
        [SugarColumn(IsNullable = false, ColumnDescription = "所属菜单", ColumnDataType = "varchar", Length = 50)]
        public string Pid { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "访问的接口ID", ColumnDataType = "varchar", Length = 50)]
        public string Fid { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "按钮名称", ColumnDataType = "varchar", Length = 50)]
        public string Name { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "调用方法名称", ColumnDataType = "varchar", Length = 50)]
        public string Function { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "图标", ColumnDataType = "varchar", Length = 50)]
        public string Icon { get; set; }

        [SugarColumn(IsNullable = true, ColumnDescription = "描述", ColumnDataType = "varchar", Length = 500)]
        public string Description { get; set; }
    }
}
