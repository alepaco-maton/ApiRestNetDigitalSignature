namespace ApiRestNetDigitalSignature.Dominio.Common;

public class DigitalSignatureException : Exception
{
    public String Code { get; }
    public String Message { get; }

    public DigitalSignatureException(String code, String message)
    {
        Code = code;
        Message = message;
    }

    public DigitalSignatureException(String code, String message, Exception cause) :
    base(cause.Message, cause)
    {
        Code = code;
        Message = message;
    }

}