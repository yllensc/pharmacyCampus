using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;
    public class PurchasedMedicineRepository: GenericRepository<PurchasedMedicine>, IPurchasedMedicine
{
    private readonly PharmacyDbContext _context;

    public PurchasedMedicineRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }

    public override async Task<IEnumerable<PurchasedMedicine>> GetAllAsync()
    {
        return await _context.PurchasedMedicines
            .Include(p=>p.Medicine)
            .ToListAsync();
    }

    public override async Task<PurchasedMedicine> GetByIdAsync(int id)
    {
        return await _context.PurchasedMedicines
            .Include(p=>p.Medicine)
            .FirstOrDefaultAsync(p=>p.Id == id);
    }
}