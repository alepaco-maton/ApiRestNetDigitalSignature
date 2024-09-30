using System.Text;
using ApiRestNetDigitalSignature.Dominio.Port;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Log;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    private static readonly ILog log = LogManager.GetLogger("ApiRest");

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync2(HttpContext context)
    {
        var transactionId = Guid.NewGuid().ToString();
        var startTime = DateTime.UtcNow;

        // Ejecutar la siguiente middleware en la pipeline
        await _next(context);

        // Request
        var request = context.Request;
        var requestBody = await ReadRequestBodyAsync(request);
        log.Info($"Request [{transactionId}]: {request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString} - IP: {context.Connection.RemoteIpAddress} - Forwarded: {context.Request.Headers["X-Forwarded-For"]} - Headers: {JsonConvert.SerializeObject(request.Headers)} - Body: {requestBody}");

        // Response
        var response = context.Response;
        var endTime = DateTime.UtcNow;
        var responseBody = await ReadResponseBodyAsync(response);
        log.Info($"Response [{transactionId}]: {response.StatusCode} {request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString} - IP: {context.Connection.RemoteIpAddress} - Forwarded: {context.Request.Headers["X-Forwarded-For"]} - Headers: {JsonConvert.SerializeObject(response.Headers)} - Body: {responseBody} - Elapsed: {endTime - startTime}");
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using (var reader = new StreamReader(request.Body))
        {
            request.Body.Position = 0;
            return await reader.ReadToEndAsync();
        }
    }

    private async Task<string> ReadResponseBodyAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var body = new MemoryStream();
        await response.Body.CopyToAsync(body);
        body.Position = 0;
        using (var reader = new StreamReader(body))
        {
            return await reader.ReadToEndAsync();
        }
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var transactionId = Guid.NewGuid().ToString();
        var startTime = DateTime.UtcNow;

        // Loguear el Request
        await LogRequestAsync(context, transactionId);

        var endTime = DateTime.UtcNow;
        // Loguear el Response
        await LogResponseAsync(context, transactionId, startTime, endTime);
    }

    private async Task LogRequestAsync(HttpContext context, string transactionId)
    {
        // Guardar el request original
        context.Request.EnableBuffering();  // Habilitar buffering para leer el stream sin cerrarlo

        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;  // Resetear la posición del stream para que el resto de la app pueda leerlo

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"------------------- REQUEST : [{transactionId}] ---------------------");
        sb.Append($"URL: {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString} - ");
        sb.Append($"IP: {context.Connection.RemoteIpAddress} - ");
        sb.Append($"Forwarded: {context.Request.Headers["X-Forwarded-For"]} - ");
        sb.Append($"Headers: {JsonConvert.SerializeObject(context.Request.Headers)} - ");
        sb.Append($"Body: {requestBody}");
        sb.AppendLine("------------------------------------------------");

        log.Info(sb.ToString());
    }

    private async Task LogResponseAsync(HttpContext context, string transactionId,
    DateTime startTime, DateTime endTime)
    {
        // Guardar el response original
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);  // Continuar con la cadena de middleware

            context.Response.Body.Seek(0, SeekOrigin.Begin);  // Resetear la posición del stream
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);  // Resetear la posición para que el resto de la app pueda leerlo

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"------------------- RESPONSE : [{transactionId}] - HTTP STATUS : " + context.Response.StatusCode + " ---------------------");
            sb.AppendLine($"Headers: {JsonConvert.SerializeObject(context.Response.Headers)} - ");
            sb.AppendLine($"Body: {text} - ");
            sb.AppendLine($"Elapsed: {endTime - startTime}");
            sb.AppendLine("------------------------------------------------");

            log.Info(sb.ToString());

            await responseBody.CopyToAsync(originalBodyStream);  // Copiar el contenido del stream al original
        }
    }
}
