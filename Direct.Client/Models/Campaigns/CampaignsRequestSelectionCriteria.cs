using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct.Client.Models.Campaigns
{
    public record CampaignsRequestSelectionCriteria(
       int[] Ids
   );
}
