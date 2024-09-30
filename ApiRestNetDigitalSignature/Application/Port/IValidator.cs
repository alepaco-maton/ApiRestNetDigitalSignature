namespace ApiRestNetDigitalSignature.Dominio.Port;

public interface IValidator<T>
{

    public string Validate(T request);

}