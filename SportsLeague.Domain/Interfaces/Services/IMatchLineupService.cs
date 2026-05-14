using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {
        /// <summary>Agrega un jugador a la alineación del partido.</summary>
        Task<MatchLineup> AddPlayerAsync(int matchId, MatchLineup lineup);

        /// <summary>Obtiene la alineación completa de un partido.</summary>
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);

        /// <summary>Obtiene la alineación de un equipo específico en un partido.</summary>
        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId);

        /// <summary>Elimina un jugador de la alineación.</summary>
        Task DeleteAsync(int lineupId);
    }
}
