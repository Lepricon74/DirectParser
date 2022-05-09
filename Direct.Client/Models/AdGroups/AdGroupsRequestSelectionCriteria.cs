namespace Direct.Client.Models.AdGroups
{
    public record AdGroupsRequestSelectionCriteria(
       long[] Ids,
       long[] CampaignIds
   );
}
