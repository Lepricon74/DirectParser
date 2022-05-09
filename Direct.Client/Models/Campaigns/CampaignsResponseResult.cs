using System.Collections.Generic;
namespace Direct.Client.Models.Campaings
{
    public record CampaignsResponseResult(
        List<CampaignResponse> Campaigns
    );
}
