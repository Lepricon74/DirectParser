using System.Collections.Generic;
using Direct.Shared.Models;

namespace Direct.Client.Models.AdImages
{
	public record AdImagesResponseResult(
		List<AdsImageModel> AdImages
	);
}