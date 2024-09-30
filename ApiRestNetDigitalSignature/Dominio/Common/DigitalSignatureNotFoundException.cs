namespace ApiRestNetDigitalSignature.Dominio.Common;

public class DigitalSignatureNotFoundException : Exception
{
    public String Code { get; }
    public String Message { get; }

    public DigitalSignatureNotFoundException(String code, String message)
    {
        Code = code;
        Message = message;
    }

    public DigitalSignatureNotFoundException(String code, String message, Exception cause) :
    base(cause.Message, cause)
    {
        Code = code;
        Message = message;
    }

}