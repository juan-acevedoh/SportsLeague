using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;


        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentRepository = tournamentRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;

        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _sponsorRepository.GetByIdAsync(id);
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            Sponsor? existing = await _sponsorRepository.GetByNameAsync(sponsor.Name);
            if (existing != null)
                throw new InvalidOperationException($"Ya existe un sponsor con el nombre {sponsor.Name}");

            return await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            Sponsor? existing = await _sponsorRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {id}");

            existing.Name = sponsor.Name;
            existing.ContactEmail = sponsor.ContactEmail;
            existing.Phone = sponsor.Phone;
            existing.WebsiteUrl = sponsor.WebsiteUrl;

            await _sponsorRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            bool exists = await _sponsorRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Sponsor con ID {id} no encontrado");

            await _sponsorRepository.DeleteAsync(id);
        }

        //Relation with Tournament 
        public async Task<TournamentSponsor> RegisterSponsorAsync(int tournamentId, int sponsorId, decimal contractAmount)
        {
            Tournament tournament = await _tournamentRepository.GetByIdAsync(tournamentId)
                ?? throw new KeyNotFoundException("Torneo no encontrado");

            Sponsor sponsor = await _sponsorRepository.GetByIdAsync(sponsorId)
                ?? throw new KeyNotFoundException("Sponsor no encontrado");

            TournamentSponsor? existing = await _tournamentSponsorRepository.GetByTournamentAndSponsorAsync(tournamentId, sponsorId);
            if (existing != null)
                throw new InvalidOperationException("El sponsor ya está registrado en este torneo");

            TournamentSponsor ts = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount
            };

            return await _tournamentSponsorRepository.CreateAsync(ts);
        }

        public async Task<IEnumerable<TournamentSponsor>> GetSponsorsByTournamentAsync(int tournamentId)
        {
            return await _tournamentSponsorRepository.GetSponsorsByTournamentAsync(tournamentId);
        }

        public async Task RemoveSponsorAsync(int tournamentId, int sponsorId)
        {
            TournamentSponsor link = await _tournamentSponsorRepository.GetByTournamentAndSponsorAsync(tournamentId, sponsorId)
                ?? throw new KeyNotFoundException("El sponsor no está registrado en este torneo");

            await _tournamentSponsorRepository.DeleteAsync(link.Id);
        }
    }
}
