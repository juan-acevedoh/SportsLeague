namespace SportsLeague.API.DTOs.Response
{
    public class TournamentSponsorResponseDTO
    {
        public int Id { get; set; }

        public int SponsorId { get; set; }
        public string SponsorName { get; set; } = string.Empty;

        public decimal ContractAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
