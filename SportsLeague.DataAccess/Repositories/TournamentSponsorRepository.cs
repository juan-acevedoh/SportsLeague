using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository
        : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tournamentId, int sponsorId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts => ts.TournamentId == tournamentId && ts.SponsorId == sponsorId);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId)
        {
            return await _dbSet
                .Include(ts => ts.Sponsor)
                .Where(ts => ts.TournamentId == tournamentId)
                .ToListAsync();
        }
    }
}
