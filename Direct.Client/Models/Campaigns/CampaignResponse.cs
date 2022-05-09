namespace Direct.Client.Models.Campaings
{
    public record CampaignResponse(
        long Id,
        string Name,
        string StartDate,
        string EndDate,
        string Type,
        string Status
    );
}
