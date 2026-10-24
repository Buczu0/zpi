using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model.DataModels;
using WebStore.Services.Interfaces;
using WebStore.Services.Validation;
using WebStore.ViewModels.VM;

namespace WebStore.Services.ConcreteServices;

public class StoreService(ApplicationDbContext db, IMapper mapper, ILogger<StoreService> logger)
    : BaseService(db, mapper, logger), IStoreService
{
    private IQueryable<StationaryStore> Query => DbContext.Stores.AsNoTracking().Include(x => x.Addresses).Include(x => x.Employees).AsSplitQuery();

    public StoreVm AddOrUpdateStore(AddOrUpdateStoreVm model)
    {
        ArgumentNullException.ThrowIfNull(model);
        Guard.Id(model.Id);
        Guard.Text(model.Name, nameof(model.Name));
        var entity = Write(() =>
        {
            var store = model.Id.GetValueOrDefault() == 0
                ? new StationaryStore()
                : DbContext.Stores.Include(x => x.Addresses).Include(x => x.Employees).AsSplitQuery()
                    .SingleOrDefault(x => x.Id == model.Id) ?? throw new KeyNotFoundException("Nie znaleziono sklepu.");
            Mapper.Map(model, store);
            if (model.Id.GetValueOrDefault() == 0) DbContext.Stores.Add(store);
            return store;
        });
        return Mapper.Map<StoreVm>(entity);
    }

    public void AssignEmployee(int employeeId, int storeId)
    {
        Write(() =>
        {
            var employee = DbContext.Employees.SingleOrDefault(x => x.Id == employeeId)
                ?? throw new KeyNotFoundException("Nie znaleziono pracownika sklepu.");
            if (!DbContext.Stores.Any(x => x.Id == storeId)) throw new KeyNotFoundException("Nie znaleziono sklepu.");
            employee.StationaryStoreId = storeId;
            return employee;
        });
    }

    public StoreVm? GetStore(Expression<Func<StationaryStore, bool>> filter) => ReadOne<StationaryStore, StoreVm>(Query, filter);
    public IReadOnlyList<StoreVm> GetStores(Expression<Func<StationaryStore, bool>>? filter = null) => ReadMany<StationaryStore, StoreVm>(Query, filter);
}
