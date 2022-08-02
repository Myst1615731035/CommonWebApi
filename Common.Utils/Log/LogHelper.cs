using log4net;
using System.Diagnostics;

namespace Common.Utils
{
    public class LogHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LogHelper));
        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        private static string GetCallerNameAndMsg(string msg)
        {
            var callerMethod = new StackFrame(2, true)?.GetMethod();
            if (callerMethod == null)
            {
                return msg;
            }

            return $"{msg}\r\n{callerMethod.DeclaringType.FullName}.{callerMethod.Name}";
        }

        public static void Info(string msg)
        {
            if(log.IsInfoEnabled && msg.IsNotEmptyOrNull())
            {
                log.Info(GetCallerNameAndMsg(msg));
            }
        }

        public static void Error(Exception ex)
        {
            if (log.IsErrorEnabled && ex.IsNotEmptyOrNull())
            {
                log.Error(GetCallerNameAndMsg(ex.Message));
            }
        }
    }
}
