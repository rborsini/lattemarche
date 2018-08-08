using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.Utils.Logs
{
    public class Logger : LogImpl
    {
        private static readonly Type DeclaringType = typeof(Logger);

        public Logger(ILoggerWrapper loggerWrapper)
            : this(loggerWrapper.Logger)
        { }

        public Logger(ILogger logger)
            : base(logger)
        { }

        protected LoggingEvent GetLoggingEvent(Level level, string message, double? duration = null, Exception exception = null, string request = "", string arguments = "")
        {
            var loggingEvent = new LoggingEvent(DeclaringType, this.Logger.Repository, this.Logger.Name, level, message, exception);
            loggingEvent.Properties["duration"] = duration.HasValue ? duration.Value.ToString() : "";

            loggingEvent.Properties["request"] = request;
            loggingEvent.Properties["arguments"] = arguments;

            return loggingEvent;
        }

        public void Debug(string message, double duration = 0, string request = "", string arguments = "")
        {
            if (!this.IsInfoEnabled)
            {
                return;
            }

            var loggingEvent = this.GetLoggingEvent(Level.Debug, message, duration, null, request, arguments);
            this.Logger.Log(loggingEvent);
        }

        public void Info(string message, double duration = 0, string request = "", string arguments = "")
        {
            if (!this.IsInfoEnabled)
            {
                return;
            }

            var loggingEvent = this.GetLoggingEvent(Level.Info, message, duration, null, request, arguments);
            this.Logger.Log(loggingEvent);
        }

        public void Error(string message, Exception exc)
        {
            MethodBase method = new StackTrace().GetFrame(1).GetMethod();

            var controllerName = method.ReflectedType.Name;
            var actionName = method.Name;

            if (!this.IsErrorEnabled)
            {
                return;
            }


            if (String.IsNullOrEmpty(message))
            {
                message = String.Format("{0}/{1} [EXCEPTION]", controllerName, actionName);
            }

            var loggingEvent = this.GetLoggingEvent(Level.Error, message, 0, exc, "", "");
            this.Logger.Log(loggingEvent);
        }

        public void Error(Exception exc)
        {
            MethodBase method = new StackTrace().GetFrame(1).GetMethod();

            var controllerName = method.ReflectedType.Name;
            var actionName = method.Name;

            if (!this.IsErrorEnabled)
            {
                return;
            }

            var message = String.Format("{0}/{1} [EXCEPTION]", controllerName, actionName);

            var loggingEvent = this.GetLoggingEvent(Level.Error, message, 0, exc, "", "");
            this.Logger.Log(loggingEvent);
        }

    }
}