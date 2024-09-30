
using ApiRestNetDigitalSignature.Dominio.Model;

namespace ApiRestNetDigitalSignature.Application.Port;

public interface ICreateCertAndPairKeyUseCase
{
    DsUser Create(DsUser dsUser, string pathFolderByUser);

}