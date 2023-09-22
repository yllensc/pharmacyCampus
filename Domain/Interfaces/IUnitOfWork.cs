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
<<<<<<< HEAD
    ISaleMedicineRepository SaleMedicines {get; }
    IMedicineRepository Medicines {get; }
    IUserRol UserRoles {get; }
<<<<<<< HEAD
=======
    IMedicineRepository Medicines {get; }
>>>>>>> 6b78404 (método GET medicine)
=======
>>>>>>> c89bc5f (Adaptación de las tablas "intermedias" jeje)
    Task<int> SaveAsync();

}
