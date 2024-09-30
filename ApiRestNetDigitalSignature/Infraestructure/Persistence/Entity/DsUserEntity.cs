using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRestNetDigitalSignature.Infraestructure.Persistence.Entity;

public class DsUserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    public string UserName { get; set; }
    public string? Cert { get; set; }
    public string? PrivateKey { get; set; }
    public string? PublicKey { get; set; }

}