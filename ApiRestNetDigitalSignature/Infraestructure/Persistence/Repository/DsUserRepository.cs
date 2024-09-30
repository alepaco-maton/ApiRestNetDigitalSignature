using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Dominio.Port;
using ApiRestNetDigitalSignature.Infraestructure.Persistence.Entity;
using AutoMapper;

namespace ApiRestNetDigitalSignature.Infraestructure.Persistence.Repository;

public class DsUserRepository : GenericRepository<DsUserEntity, DsUser, int?>, IDsUserRepository
{
    public DsUserRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public override int? GetIdByModel(DsUser model)
    {
        return model.Id;
    }

    public List<DsUser> GetAllByUserName(string? userName)
    {
        return _mapper.Map<List<DsUser>>(_context.DsUsersEntity.Where(p => p.UserName.Equals(userName)));
    }

}