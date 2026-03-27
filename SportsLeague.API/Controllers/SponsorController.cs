using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _sponsorService;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService sponsorService, IMapper mapper)
        {
            _sponsorService = sponsorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
        {
            IEnumerable<Sponsor> sponsors = await _sponsorService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
        {
            Sponsor? sponsor = await _sponsorService.GetByIdAsync(id);
            if (sponsor == null) return NotFound();

            return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));
        }

        [HttpPost]
        public async Task<ActionResult> Create(SponsorRequestDTO dto)
        {
            try
            {
                Sponsor entity = _mapper.Map<Sponsor>(dto);
                Sponsor? created = await _sponsorService.CreateAsync(entity);

                SponsorResponseDTO result = _mapper.Map<SponsorResponseDTO>(created);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Add new Tournament
        [HttpPost("{tournamentId}/sponsors")]
        public async Task<ActionResult> RegisterSponsor(int tournamentId, TournamentSponsorRequestDTO dto)
        {
            try
            {
                TournamentSponsor result = await _sponsorService.RegisterSponsorAsync(
                    tournamentId,
                    dto.SponsorId,
                    dto.ContractAmount
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
