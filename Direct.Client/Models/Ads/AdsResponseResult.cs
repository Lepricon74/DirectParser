using System.Collections.Generic;

namespace Direct.Client.Models.Ads
{
    public record AdsResponseResult(
        List<AdShortModel> Ads
    );
}
