namespace Direct.Client.Models.Ads
{
    public record AdsRequestSelectionCriteria(
       long[] Ids,
       long[] AdGroupIds,
       long[] CampaignIds
   );
}
