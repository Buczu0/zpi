using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;

namespace WebStore.Services.ConcreteServices;

public abstract class BaseService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
{
    protected readonly ApplicationDbContext DbContext = dbContext;
    protected readonly IMapper Mapper = mapper;
    protected readonly ILogger Logger = logger;

    protected T Write<T>(Func<T> operation)
    {
        // Walidacja i odczyt reguł należą do tej samej transakcji co zapis.
        // Serializable chroni m.in. zamówienie przed równoczesnym fakturowaniem i edycją.
        IDbContextTransaction? transaction = null;
        try
        {
            if (DbContext.Database.IsRelational())
                transaction = DbContext.Database.BeginTransaction(IsolationLevel.Serializable);
            // Nie używaj kopii z poprzednich operacji do sprawdzania bieżących reguł.
            // Serializable chroni odczyty SQL, ale nie odświeża pamięci kontekstu EF.
            // Odpinamy tylko niezmienione encje, zachowując lokalne zmiany oczekujące.
            foreach (var entry in DbContext.ChangeTracker.Entries()
                         .Where(entry => entry.State == EntityState.Unchanged).ToArray())
                entry.State = EntityState.Detached;
            var result = operation();
            DbContext.SaveChanges();
            transaction?.Commit();
            return result;
        }
        catch (Exception ex)
        {
            transaction?.Rollback();
            DbContext.ChangeTracker.Clear();
            if (ex is DbUpdateException or DbException)
                Logger.LogError(ex, "Nie udało się zapisać danych usługi.");
            if (ex is DbUpdateConcurrencyException)
                throw new InvalidOperationException("Dane zostały równocześnie zmienione. Ponów operację po ich odczycie.", ex);
            throw;
        }
        finally { transaction?.Dispose(); }
    }

    protected TVm? ReadOne<TEntity, TVm>(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
        where TEntity : class where TVm : class
    {
        ArgumentNullException.ThrowIfNull(filter);
        return Read(() =>
        {
            var entity = query.Where(filter).OrderBy(x => EF.Property<int>(x, "Id")).FirstOrDefault();
            return entity is null ? null : Mapper.Map<TVm>(entity);
        });
    }

    protected IReadOnlyList<TVm> ReadMany<TEntity, TVm>(IQueryable<TEntity> query, Expression<Func<TEntity, bool>>? filter)
        where TEntity : class
        => Read(() => (IReadOnlyList<TVm>)Mapper.Map<List<TVm>>(
            (filter is null ? query : query.Where(filter)).OrderBy(x => EF.Property<int>(x, "Id")).ToList()));

    private T Read<T>(Func<T> operation)
    {
        try { return operation(); }
        catch (DbException ex)
        {
            Logger.LogError(ex, "Nie udało się odczytać danych usługi.");
            throw;
        }
    }
}
