using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class LogEntity
    {
        /// <summary>
        /// IP
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string LogInfo { get; set; }
        /// <summary>
        /// 跟踪信息
        /// </summary>
        public string StackTrace { get; set; }
    }
}
