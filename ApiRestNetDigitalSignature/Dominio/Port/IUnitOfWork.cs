namespace ApiRestNetDigitalSignature.Dominio.Port;

public interface IUnitOfWork
{
    IDsUserRepository DsUserRepository { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync();
    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}