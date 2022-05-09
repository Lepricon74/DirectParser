using System.Collections.Generic;

namespace Direct.Client.Models.AdGroups
{
    public record AdGroupsResponseResult(
        List<AdGroupShortModel> AdGroups
    );
}
