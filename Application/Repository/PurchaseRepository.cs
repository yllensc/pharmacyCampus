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
}