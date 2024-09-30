using ApiRestNetDigitalSignature.Application.Service.Cypher;
using ApiRestNetDigitalSignature.Dominio.Model;

namespace ApiRestNetDigitalSignature.Dominio.Port;

public interface IDsUserRepository : IDisposable
{
    List<DsUser> GetAllByUserName(string? userName);
    Task<DsUser> AddAsync(DsUser model);
    Task<DsUser> UpdateAsync(DsUser model);
    void SaveChanges();
}