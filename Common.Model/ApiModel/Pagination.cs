using Newtonsoft.Json.Linq;

namespace Common.Model.ApiModel
{
    /// <summary>
    /// 通用分页信息类
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string keyword { get; set; } = "";
        /// <summary>
        /// 当前页标
        /// </summary>
        public int currentPage { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { get; set; } = 1;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int total { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int pageSize { set; get; } = 30;
        /// <summary>
        /// 表单查询
        /// </summary>
        public JObject form { get; set; }
        /// <summary>
        /// 是否查询全部
        /// </summary>
        public bool isAll { get; set; }
        /// <summary>
        /// 是否作为选项查询
        /// </summary>
        public bool isOption { get; set; } = false;
        /// <summary>
        /// 排序
        /// </summary>
        public string sort { get; set; } = "";
        /// <summary>
        /// 返回数据
        /// </summary>
        public object? response { get; set; }

        /// <summary>
        /// 其他数据内容
        /// </summary>
        public object? other { get; set; }
    }

    public class Option
    {
        public int value { get; set; }
        public string label { get; set; }
    }
}
