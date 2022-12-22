using System;

namespace Direct.Parser.Database.Models
{
	public class AdImage
	{
		public string ImageHash { get; set; }
		public string ImageUrl { get; set; }
		public string ImageText { get; set; }
		public DateTime? PromotionEndDate { get; set; }

		public AdImage(
			string imageHash,
			string imageUrl,
			string imageText,
			DateTime? promotionEndDate)
		{
			ImageHash = imageHash;
			ImageUrl = imageUrl;
			ImageText = imageText;
			PromotionEndDate = promotionEndDate;
		}
	}
}