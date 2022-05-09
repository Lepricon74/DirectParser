namespace Direct.Client.Models.AdGroups
{
    public record AdGroupShortModel(
        long Id,
        string Name,
        long CampaignId,
        string Type,
        string Status
    );
}
