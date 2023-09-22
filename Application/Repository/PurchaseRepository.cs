using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
    public class PurchaseRepository: GenericRepository<Purchase>, IPurchase
{
    private readonly PharmacyDbContext _context;

    public PurchaseRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }

    public async Task<Purchase> GetByDate(DateTime date)
    {
        return await _context.Purchases
                    .Include(p=>p.PurchasedMedicines)
                    .FirstAsync(u => u.DatePurchase == date);
    }

    public override async Task<IEnumerable<Purchase>> GetAllAsync()
    {
        return await _context.Purchases
            .Include(p=>p.PurchasedMedicines).ThenInclude(p=>p.Medicine)
            .ToListAsync();
    }

    public override async Task<Purchase> GetByIdAsync(int id)
    {
        return await _context.Purchases
            .Include(p=>p.PurchasedMedicines).ThenInclude(p=>p.Medicine)
            .FirstOrDefaultAsync(p=>p.Id == id);
    }
}