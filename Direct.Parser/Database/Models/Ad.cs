using System;
using Direct.Client.Models.Ads;

namespace Direct.Parser.Database.Models
{
    public record Ad(
        long Id,
        long AdGroupId,
        long CampaignId,
        string Type,
        string Status,
        string TextAd,
        string Title,
        DateTime? promotionEndDate); 
}
