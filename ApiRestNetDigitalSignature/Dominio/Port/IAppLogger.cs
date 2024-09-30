namespace ApiRestNetDigitalSignature.Dominio.Port;

public interface IAppLogger
{

    void Info(string message);

    void Info(string message, Exception exception);

    void Warn(string message);

    void Warn(string message, Exception exception);

    void Error(string message);

    void Error(string message, Exception exception);

}