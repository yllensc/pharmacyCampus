using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repository;

public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
{
    private readonly PharmacyDbContext _context;

    public MedicineRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }

    public override async Task<Medicine> GetByIdAsync(int id)
    {
        return await _context.Medicines
                    .Include(p => p.PurchasedMedicines)
                    .FirstOrDefaultAsync(p=>p.Id == id);
    }
}
