using ApiRestNetDigitalSignature.Dominio.Model;

namespace ApiRestNetDigitalSignature.Application.Port;

public interface ICreateDsUserService
{
    Task<DsUser> Create(DsUser model, string pathFolderByUser);

}