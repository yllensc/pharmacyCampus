using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
    public class ProviderRepository: GenericRepository<Provider>, IProvider
{
    private readonly PharmacyDbContext _context;

    public ProviderRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }
     public override async Task<IEnumerable<Provider>> GetAllAsync()
    {
        return await _context.Providers
            .Include(p=>p.Purchases).ThenInclude(p=>p.PurchasedMedicines).ThenInclude(p=>p.Medicine)
            .ToListAsync();
    }

    public override async Task<Provider> GetByIdAsync(int id)
    {
        return await _context.Providers
            .Include(p=>p.Purchases).ThenInclude(p=>p.PurchasedMedicines)
            .FirstOrDefaultAsync(p=>p.Id == id);
    }
}