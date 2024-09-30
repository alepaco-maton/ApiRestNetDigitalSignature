namespace ApiRestNetDigitalSignature.Infraestructure.ApiRestDigitalSignature.Dto.User;

public class CreateUserResponse
{

    public int? Id { get; set; }
    public string UserName { get; set; }

    public CreateUserResponse(int? id, string userName)
    {
        Id = id;
        UserName = userName;
    }
}