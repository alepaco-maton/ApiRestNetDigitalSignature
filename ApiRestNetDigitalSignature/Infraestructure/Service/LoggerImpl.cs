
using System.Diagnostics;
using System.Reflection;
using System.Text;
using ApiRestNetDigitalSignature.Dominio.Port;
using log4net;

namespace ApiRestNetDigitalSignature.Infraestructure.Service;

public class LoggerImpl : IAppLogger
{
    private ILog log = LogManager.GetLogger("Application");

    public void Info(string message)
    {
        log = GetLogger();
        log.Info(message);
    }

    public void Info(string message, Exception exception)
    {
        log = GetLogger();
        log.Info(message, exception);
    }

    public void Warn(string message)
    {
        log = GetLogger();
        log.Warn(message);
    }

    public void Warn(string message, Exception exception)
    {
        log = GetLogger();
        log.Warn(message, exception);
    }

    public void Error(string message)
    {
        log = GetLogger();
        log.Error(message);
    }

    public void Error(string message, Exception exception)
    {
        log = GetLogger();
        log.Error(message, exception);
    }

    private ILog GetLogger()
    {
        var frame = new StackFrame(2, false); // 2 niveles para obtener la clase llamante 
        MethodBase method = frame.GetMethod();

        StringBuilder sb = new StringBuilder("Application.");
        sb.Append(method.DeclaringType);
        sb.Append(".");
        sb.Append(method.Name);
        // sb.Append(".");
        // sb.Append(frame.GetFileLineNumber().ToString());

        return LogManager.GetLogger(sb.ToString());
    }
}