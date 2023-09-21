using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using Domain.Interfaces;
using Persistence;

namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly PharmacyDbContext _context;
    private IRolRepository _roles;
    private IUserRepository _users;
    private IEmployee _employees;
    private IMedicineRepository _medicines;
    private ISaleMedicineRepository _saleMedicines;
    private IPurchase _purchase;
    private IPurchasedMedicine _purchasedMedicine;
    private IProvider _provider;
<<<<<<< HEAD
<<<<<<< HEAD
    private IPosition _position;
    private IPatient _patients;
=======
>>>>>>> 5c89ca3 (delete migrations aaaaaaaa)
=======
    private IPatient _patients;
>>>>>>> dcfa50b (feat: :sparkles: Add patient to UnitOfWork)
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
        public IPosition Positions
    {
        get
        {
            if (_position == null)
            {
                _position = new PositionRepository(_context);
            }
            return _position;
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
public IProvider Providers
    {
        get{
            if(_provider == null)
            {
                _provider = new ProviderRepository(_context);
            }
            return _provider;
        }
    }

<<<<<<< HEAD
<<<<<<< HEAD
=======
    public IProvider Provider {
        get{
            if(_provider == null)
            {
                _provider = new ProviderRepository(_context);
            }
            return _provider;
        }
    }
>>>>>>> eaebbce (feat: :sparkles: Creacion Interfaces y repo)
=======

>>>>>>> 21a3290 (arreglando el bololó de mi commit :ccc)
    public IPatient Patients
    {
         get{
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


    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}