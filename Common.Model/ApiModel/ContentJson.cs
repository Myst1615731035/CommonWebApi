using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model.ApiModel
{
    public class ContentJson<T> where T : class
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; } = 200;

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get; set; } = false;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "Fail";

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; } = null;
    }
}
