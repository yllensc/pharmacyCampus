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
    private IUserRol _userole;
    private IEmployee _employees;
    private IMedicineRepository _medicines;
    private ISaleMedicineRepository _saleMedicines;
    private IPurchase _purchase;
    private IPurchasedMedicine _purchasedMedicine;
    private IProvider _provider;
    private IPosition _position;
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
    public IUserRol UserRoles
    {
        get
        {
            if (_userole == null)
            {
                _userole = new UseroleRepository(_context);
            }
            return _userole;
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

<<<<<<< HEAD
<<<<<<< HEAD
    public ISaleMedicineRepository SaleMedicines
=======
    public ISaleMedicineRepository saleMedicine
>>>>>>> 3faabab (feat: :construction: Providers + Purchases + Medicines...)
    {
        get{
            if(_saleMedicines == null)
            {
                _saleMedicines = new SaleMedicineRepository(_context);
            }
            return _saleMedicines;
        }
<<<<<<< HEAD
    }
=======
>>>>>>> 3faabab (feat: :construction: Providers + Purchases + Medicines...)

    }
=======
>>>>>>> c89bc5f (Adaptación de las tablas "intermedias" jeje)
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}