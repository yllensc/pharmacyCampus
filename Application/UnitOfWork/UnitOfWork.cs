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
    private IPatient _patients;
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

    public IPurchase Purchases => throw new NotImplementedException();

    public IPurchasedMedicine PurchasedMedicines => throw new NotImplementedException();

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