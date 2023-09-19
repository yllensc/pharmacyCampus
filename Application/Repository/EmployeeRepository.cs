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

    public async Task<Employee> GetByEmployeeIDAsync(string idenNumber)
    {
         return await _context.Employees
            .Include(u => u.Sales)
            .FirstOrDefaultAsync(u => u.IdenNumber.ToLower() == idenNumber.ToLower());
    }
}
