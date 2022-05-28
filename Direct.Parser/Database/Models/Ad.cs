using System;

namespace Direct.Parser.Database.Models
{
    public class Ad 
    {
        public long Id { get; set; }
        public long AdGroupId { get; set; }
        public long CampaignId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string TextAd { get; set; }
        public string Title { get; set; }
        public DateTime? promotionEndDate { get; set; }

        public Ad(
            long Id, 
            long AdGroupId, 
            long CampaignId, 
            string Type, 
            string Status, 
            string TextAd, 
            string Title, 
            DateTime? promotionEndDate) {
            this.Id = Id;
            this.AdGroupId = AdGroupId;
            this.CampaignId = CampaignId;
            this.Type = Type;
            this.Status = Status;
            this.TextAd = TextAd;
            this.Title = Title; 
            this.promotionEndDate = promotionEndDate;
        }
    }
}
