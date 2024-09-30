using ApiRestNetDigitalSignature.Infraestructure.Persistence.Entity;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace ApiRestNetDigitalSignature.Infraestructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<DsUserEntity> DsUsersEntity { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

}