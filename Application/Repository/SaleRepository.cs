using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.Repository;

public class SaleRepository : GenericRepository<Sale>, ISale
{
    private readonly PharmacyDbContext _context;

    public SaleRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;

    }
}
