namespace Direct.Client.Models.Ads
{
    public record AdShortModel(
        long Id,
        long AdGroupId,
        long CampaignId,
        string Type,
        string Status,
        TextAd TextAd
    );
}
