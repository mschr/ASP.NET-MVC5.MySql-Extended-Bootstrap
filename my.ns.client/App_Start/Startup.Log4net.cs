using System;
using log4net;
using System.Reflection;
using System.Diagnostics;
using config = System.Configuration.ConfigurationManager;

namespace my.ns.client
{
    public partial class Startup
    {
        public void ConfigureLogger()
        {
            bool enabled = false;
            if(bool.TryParse(config.AppSettings["log4net.redirectTraceToAppenders"], out enabled)) 
                if(enabled)
                   Trace.Listeners.Add(new Log4netTraceListener());

            if (bool.TryParse(config.AppSettings["log4net.traceExtensionEnabled"], out enabled))
            {
                ILogExtentions.TraceEnabled = enabled;
            }

        }
    }

    #region logextension: Trace
    public static class ILogExtentions
    {
        internal static bool TraceEnabled = true;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Trace(this ILog log, string message, Exception exception)
        {
            if (!TraceEnabled) return;
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                log4net.Core.Level.Trace, message, exception);
        }

        public static void Trace(this ILog log, string message)
        {
            if (!TraceEnabled) return;
            log.Trace(message, null);
        }
    }
    #endregion

    #region tracelistener_to_logger
    public class Log4netTraceListener : TraceListener
    {
        private readonly log4net.ILog _log;

        #region GetTracingStackFrame
        private StackFrame GetTracingStackFrame(StackTrace stack)
        {
            for (var i = 0; i < stack.FrameCount; i++)
            {
                var frame = stack.GetFrame(i);
                var method = frame.GetMethod();
                if (null == method)
                {
                    continue;
                }

                if ("System.Diagnostics" == method.DeclaringType.Namespace)
                {
                    continue;
                }

                if ("System.Threading" == method.DeclaringType.Namespace)
                {
                    continue;
                }

                if (this.GetType() == method.DeclaringType)
                {
                    continue;
                }

                return stack.GetFrame(i);
            }

            return null;
        }
        #endregion

        public Log4netTraceListener()
        {
            _log = log4net.LogManager.GetLogger("System.Diagnostics.Redirection");
        }

        public Log4netTraceListener(log4net.ILog log)
        {
            _log = log;
        }

        public override void Write(string message)
        {
            if (_log != null)
            {
                try
                {
                    StackTrace st = new StackTrace();

                    var method = this.GetTracingStackFrame(st).GetMethod();
                    var declaringType = method.DeclaringType;
                    if (declaringType == typeof(log4net.Util.LogLog))
                    {
                        return;
                    }
                    log4net.LogManager.GetLogger(declaringType).Trace(message);
                    //_log.Trace(message);
                }
                catch { }

            }
        }

        public override void WriteLine(string message)
        {
            this.Write(message);
        }
    }
    #endregion
}
