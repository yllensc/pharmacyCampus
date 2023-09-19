using Domain.Entities;
using Domain.Interfaces;
using Persistence;

namespace Application.Repository;
    public class PositionRepository: GenericRepository<Position>, IPosition
{
    private readonly PharmacyDbContext _context;

    public PositionRepository(PharmacyDbContext context) : base(context)
    {
        _context = context;
    }


}
