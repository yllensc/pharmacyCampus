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
    IPatient Patients { get; }
    ISale Sales { get; }
    IProvider Providers {get; }
    IPurchase Purchases {get; }
    IPurchasedMedicine PurchasedMedicines {get; }
    IPatient Patients { get; }
    Task<int> SaveAsync();

}
