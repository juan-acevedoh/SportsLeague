using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
    {
        /// <summary>Obtiene toda la alineación de un partido, con Player y Team cargados.</summary>
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);

        /// <summary>Obtiene la alineación de un equipo específico en un partido.</summary>
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);

        /// <summary>Verifica si un jugador ya está registrado en la alineación de un partido.</summary>
        Task<bool> ExistsByMatchAndPlayerAsync(int matchId, int playerId);

        /// <summary>Cuenta los titulares de un equipo en un partido.</summary>
        Task<int> CountStartersByMatchAndTeamAsync(int matchId, int teamId);
    }
}
