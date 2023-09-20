using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly PharmacyDbContext _context;
    private IRolRepository _roles;
    private IUserRepository _users;
    private IEmployee _employees;
    private IPatient _patients;
    private IMedicineRepository _medicines;
    private ISaleMedicineRepository _saleMedicines;
<<<<<<< HEAD
=======
    private IPurchase _purchase;
    private IPurchasedMedicine _purchasedMedicine;
    private IProvider _provider;
<<<<<<< HEAD
<<<<<<< HEAD
    private IPatient _patients;
    private IPatient _patients;
>>>>>>> 0810f6b (feat: :sparkles: Add patient to UnitOfWork)
=======
>>>>>>> 2a7d223 (delete migrations aaaaaaaa)
=======
    private IMedicineRepository _medicines;
    private ISaleMedicineRepository _saleMedicines;
>>>>>>> f0c27a1 (feat: :sparkles: Creacion Interfaces y repo)

    private ISale _sales;
    public UnitOfWork(PharmacyDbContext context)
    {
        _context = context;
    }
    public IRolRepository Roles
    {
        get
        {
            if (_roles == null)
            {
                _roles = new RolRepository(_context);
            }
            return _roles;
        }
    }

    public IUserRepository Users
    {
        get
        {
            if (_users == null)
            {
                _users = new UserRepository(_context);
            }
            return _users;
        }
    }
    public IEmployee Employees
    {
        get
        {
            if (_employees == null)
            {
                _employees = new EmployeeRepository(_context);
            }
            return _employees;
        }
    }

    public IPatient Patients
    {
        get
        {
            if(_patients == null)
            {
                _patients = new PatientRepository(_context);
            }
            return _patients;
        }
    }

    public ISale Sales
    {
        get{
            if(_sales == null)
            {
                _sales = new SaleRepository(_context);
            }
            return _sales;
        }
    }

    public IMedicineRepository Medicines
    {
        get{
            if(_medicines == null)
            {
                _medicines = new MedicineRepository(_context);
            }
            return _medicines;
        }
    }

    public ISaleMedicineRepository SaleMedicines
    {
        get{
            if(_saleMedicines == null)
            {
                _saleMedicines = new SaleMedicineRepository(_context);
            }
            return _saleMedicines;
        }
    }
<<<<<<< HEAD
=======

    public IProvider Providers {
        get{
            if(_provider == null)
            {
                _provider = new ProviderRepository(_context);
            }
            return _provider;
        }
    }

    public IPurchase Purchases {
        get{
            if(_purchase == null)
            {
                _purchase = new PurchaseRepository(_context);
            }
            return _purchase;
        }
    }

    public IPurchasedMedicine PurchasedMedicines {
        get{
            if(_purchasedMedicine == null)
            {
                _purchasedMedicine = new PurchasedMedicineRepository(_context);
            }
            return _purchasedMedicine;
        }
    }

<<<<<<< HEAD
<<<<<<< HEAD

    public IPatient Patients
    {
        get
        {
            if(_patients == null)
            {
                _patients = new PatientRepository(_context);
            }
            return _patients;
        }
    }

    public IPatient Patients
    {
        get
        {
            if(_patients == null)
            {
                _patients = new PatientRepository(_context);
            }
            return _patients;
        }
    }
>>>>>>> 0810f6b (feat: :sparkles: Add patient to UnitOfWork)
=======
>>>>>>> 2a7d223 (delete migrations aaaaaaaa)
=======

    public IMedicineRepository Medicines
    {
        get{
            if(_medicines == null)
            {
                _medicines = new MedicineRepository(_context);
            }
            return _medicines;
        }
    }

    public ISaleMedicineRepository SaleMedicines
    {
        get{
            if(_saleMedicines == null)
            {
                _saleMedicines = new SaleMedicineRepository(_context);
            }
            return _saleMedicines;
        }
    }
>>>>>>> f0c27a1 (feat: :sparkles: Creacion Interfaces y repo)
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
