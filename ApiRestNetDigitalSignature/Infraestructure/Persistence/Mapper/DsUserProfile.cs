using ApiRestNetDigitalSignature.Dominio.Model;
using ApiRestNetDigitalSignature.Infraestructure.Persistence.Entity;
using AutoMapper;

namespace ApiRestNetDigitalSignature.Infraestructure.Persistence.Mapper;

public class DsUserProfile : Profile
{
    public DsUserProfile()
    {
        CreateMap<DsUserEntity, DsUser>();
        CreateMap<DsUser, DsUserEntity>();
    }
}