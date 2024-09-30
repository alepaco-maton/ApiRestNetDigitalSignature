namespace ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Dto.User;

public class CreateUserRequest
{
    public string UserName { get; set; }

    public CreateUserRequest(string userName)
    {
        UserName = userName;
    }
}