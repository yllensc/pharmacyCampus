using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IRolRepository Roles { get; }
    IUserRepository Users { get; }
    IEmployee Employees { get; }
    IPosition Positions {get; }
    IPatient Patients { get; }
    ISale Sales { get; }
    IProvider Providers {get; }
    IPurchase Purchases {get; }
    IPurchasedMedicine PurchasedMedicines {get; }
    ISaleMedicineRepository SaleMedicines {get; }
    IMedicineRepository Medicines {get; }
    IUserRol UserRoles {get; }
    Task<int> SaveAsync();

}
