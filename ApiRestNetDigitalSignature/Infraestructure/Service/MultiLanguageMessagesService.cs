
using System.Reflection;
using System.Resources;
using ApiRestNetDigitalSignature.Dominio.Port;

namespace ApiRestNetDigitalSignature.Infraestructure.Service;

public class MultiLanguageMessagesService : IMultiLanguageMessagesService
{
    ResourceManager messageSource = new ResourceManager("ApiRestNetDigitalSignature.Language.strings", Assembly.GetExecutingAssembly());
    public string? GetMessage(string code)
    {
        return messageSource.GetString(code, null);
    }
}