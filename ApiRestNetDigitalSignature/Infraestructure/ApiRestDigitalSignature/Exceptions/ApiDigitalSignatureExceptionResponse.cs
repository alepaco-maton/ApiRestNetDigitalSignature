
using System.Net;

namespace ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Exceptions;

public class ApiDigitalSignatureExceptionResponse
{

    public int Status { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }

    public ApiDigitalSignatureExceptionResponse(int status, string code, string message)
    {
        Status = status;
        Code = code;
        Message = message;
    }
}