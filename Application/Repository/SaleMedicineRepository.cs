using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.Repository;

public class SaleMedicineRepository : GenericRepository<SaleMedicine>, ISaleMedicineRepository
{
    private readonly PharmacyDbContext _context;

    public SaleMedicineRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }
}
