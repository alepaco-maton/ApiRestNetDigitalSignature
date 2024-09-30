
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ApiRestNetDigitalSignature.Infraestructure.Persistence.Repository;

public abstract class GenericRepository<TEntity, TModel, TID> : IDisposable where TEntity : class where TModel : class
{
    protected bool disposed = false;
    protected readonly AppDbContext _context;
    protected readonly IMapper _mapper;
    protected DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _mapper = mapper;
    }

    public abstract TID GetIdByModel(TModel model);

    protected async Task<IEnumerable<TModel>> Get(
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return _mapper.Map<List<TModel>>(orderBy(query).ToList());
        }
        else
        {
            return _mapper.Map<List<TModel>>(query.ToList());
        }
    }

    public virtual async Task<TModel> GetByIdAsync(TID id)
    {
        var entity = await _dbSet.FindAsync(id);
        return _mapper.Map<TModel>(entity);
    }

    public virtual async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync<TEntity>();
        return _mapper.Map<List<TModel>>(entities);
    }

    public virtual async Task<TModel> AddAsync(TModel model)
    {
        var entity = _mapper.Map<TEntity>(model);
        model = _mapper.Map<TModel>((await _dbSet.AddAsync(entity)).Entity);
        await _context.SaveChangesAsync();
        return model;
    }

    public virtual async Task<TModel> UpdateAsync(TModel model)
    {
        var entity = await _dbSet.FindAsync(GetIdByModel(model));
        entity = _mapper.Map(model, entity);
        _dbSet.Attach(entity);
        _dbSet.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return model;
    }

    public virtual async void DeleteAsync(TID id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}