using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/match/{matchId}/lineup")]
    public class MatchLineupController : ControllerBase
    {
        private readonly IMatchLineupService _lineupService;
        private readonly IMapper _mapper;

        public MatchLineupController(
            IMatchLineupService lineupService,
            IMapper mapper)
        {
            _lineupService = lineupService;
            _mapper = mapper;
        }

        // POST /api/match/{matchId}/lineup
        [HttpPost]
        public async Task<ActionResult<MatchLineupResponse>> AddPlayer(
            int matchId, MatchLineupRequest dto)
        {
            try
            {
                var lineup = _mapper.Map<MatchLineup>(dto);
                var created = await _lineupService.AddPlayerAsync(matchId, lineup);

                // Recargar con detalles para el response (Player + Team)
                var all = await _lineupService.GetByMatchAsync(matchId);
                var createdWithDetails = all.FirstOrDefault(l => l.Id == created.Id);

                var responseDto = _mapper.Map<MatchLineupResponse>(createdWithDetails);
                return StatusCode(201, responseDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET /api/match/{matchId}/lineup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchLineupResponse>>> GetByMatch(int matchId)
        {
            try
            {
                var lineups = await _lineupService.GetByMatchAsync(matchId);
                return Ok(_mapper.Map<IEnumerable<MatchLineupResponse>>(lineups));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET /api/match/{matchId}/lineup/team/{teamId}
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponse>>> GetByMatchAndTeam(
            int matchId, int teamId)
        {
            try
            {
                var lineups = await _lineupService.GetByMatchAndTeamAsync(matchId, teamId);
                return Ok(_mapper.Map<IEnumerable<MatchLineupResponse>>(lineups));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE /api/match/{matchId}/lineup/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int matchId, int id)
        {
            try
            {
                await _lineupService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
