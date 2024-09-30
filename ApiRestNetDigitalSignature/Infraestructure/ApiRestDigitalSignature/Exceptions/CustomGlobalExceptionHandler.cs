using System.Text;
using ApiRestNetDigitalSignature.Dominio.Common;
using ApiRestNetDigitalSignature.Dominio.Port;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Exceptions;

public class CustomGlobalExceptionHandler : ExceptionFilterAttribute
{
    private readonly IMultiLanguageMessagesService _mlms;
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public CustomGlobalExceptionHandler(IMultiLanguageMessagesService mlms)
    {
        _mlms = mlms;
    }

    public override void OnException(ExceptionContext context)
    {
        {
            if (context.Exception is DigitalSignatureException exception)
            {
                var response = new ApiDigitalSignatureExceptionResponse(
                    StatusCodes.Status422UnprocessableEntity,
                    exception.Code,
                    exception.Message
                );

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };

                context.ExceptionHandled = true;

                return;
            }
        }
        {
            if (context.Exception is DigitalSignatureNotFoundException exception)
            {
                var response = new ApiDigitalSignatureExceptionResponse(
                    StatusCodes.Status404NotFound,
                    exception.Code,
                    exception.Message
                );

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };

                context.ExceptionHandled = true;

                return;
            }
        }
        {
            log.Error(context.Exception.Message, context.Exception);

            var response = new ApiDigitalSignatureExceptionResponse(
                    StatusCodes.Status422UnprocessableEntity,
                    ErrorCode.ERROR_PROCESSING_THE_TRANSACTION,
                    _mlms.GetMessage(ErrorCode.ERROR_PROCESSING_THE_TRANSACTION)
                );


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-------------------RESPONSE - HTTP STATUS : " + StatusCodes.Status422UnprocessableEntity + " ---------------------");
            sb.AppendLine("DATA " + response + ", ");
            sb.AppendLine("------------------------------------------------");

            log.Info(sb.ToString());

            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };

            context.ExceptionHandled = true;

        }
    }

}