using ApiRestNetDigitalSignature.Dominio.Port;

namespace ApiRestNetDigitalSignature.Infraestructure.Persistence;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IConfiguration _configuration;
    private bool _isBdMemory;
    private bool _disposed;
    private readonly AppDbContext _dbContext;
    public IDsUserRepository DsUserRepository { get; }

    public UnitOfWork(IConfiguration configuration, AppDbContext dbContext, IDsUserRepository dsUserRepository)
    {
        _configuration = configuration;
        _disposed = false;
        _dbContext = dbContext;
        DsUserRepository = dsUserRepository;
        _isBdMemory = _configuration.GetValue<bool>("IsBdMemory");
    }
    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void BeginTransaction()
    {
        if (!_isBdMemory)
            _dbContext.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        if (!_isBdMemory)
            _dbContext.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        if (!_isBdMemory)
            _dbContext.Database.RollbackTransaction();
    }

    public virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}