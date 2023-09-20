using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;


namespace Application.Repository;
    public class EmployeeRepository: GenericRepository<Employee>, IEmployee
{
    private readonly PharmacyDbContext _context;

    public EmployeeRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;
    }

}
