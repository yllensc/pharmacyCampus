using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.Repository;
    public class PurchasedMedicineRepository: GenericRepository<PurchasedMedicine>, IPurchasedMedicine
{
    private readonly PharmacyDbContext _context;

    public PurchasedMedicineRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }
}