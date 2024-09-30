namespace ApiRestNetDigitalSignature.Dominio.Model;

public class DsUser
{

    public int? Id { get; set; }
    public string? UserName { get; set; }
    public string? Cert { get; set; }
    public string? PrivateKey { get; set; }
    public string? PublicKey { get; set; }

    public DsUser(int? id, string? userName)
    {
        this.Id = id;
        this.UserName = userName;
    }

    public DsUser(string? userName)
    {
        this.UserName = userName;
    }


}