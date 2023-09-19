using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.Repository;

public class RolRepository : GenericRepository<Rol>, IRolRepository
{
    private readonly PharmacyDbContext _context;

    public RolRepository(PharmacyDbContext context) : base(context)
    {
       _context = context;
    }
}
