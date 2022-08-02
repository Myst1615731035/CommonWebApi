using SqlSugar;

namespace Common.Model.Base
{
    public class RootEntity<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// 建议使用GUID作为主键
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true,ColumnDataType = "varchar", Length = 50, ColumnDescription = "主键")]
        public Tkey Id { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "排序", DefaultValue = "0")]
        public int Sort { get; set; }

        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "创建人主键")]
        public Tkey CreateId { get; set; }

        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "创建人姓名")]
        public string CreateName { get; set; }

        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDataType = "datetime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "创建人主键")]
        public Tkey ModifyId { get; set; }

        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDataType = "varchar", Length = 50, ColumnDescription = "创建人姓名")]
        public string ModifyName { get; set; }

        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true, ColumnDataType = "datetime", ColumnDescription = "创建时间")]
        public DateTime ModifyTime { get; set; }

        [SugarColumn(IsNullable = false, ColumnDescription = "是否逻辑删除，默认为否(0)", DefaultValue = "0")]
        public bool IsDelete { get; set; }

        [SugarColumn(IsOnlyIgnoreInsert = true, IsOnlyIgnoreUpdate = true, ColumnDescription = "乐观锁", ColumnDataType = "timestamp", IsEnableUpdateVersionValidation = true)]
        public byte[] Version { get; set; }
    }
}
