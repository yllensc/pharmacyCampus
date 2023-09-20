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

    public async Task<Provider> GetByIdProviderAsync(int id)
    {
        return await _context.Providers
                .Include(p=>p.Purchases)
                .Include(p=>p.Medicines)
                .FirstOrDefaultAsync(p => p.Id == id);
    }
}