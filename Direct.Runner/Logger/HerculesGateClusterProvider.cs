using System;
using Vostok.Clusterclient.Core.Topology;
using System.Collections.Generic;

namespace Direct.Runner.Logger
{
    public class HerculesGateClusterProvider: IClusterProvider
    {
        private readonly List<Uri>  clusterUriList;

        public HerculesGateClusterProvider(Uri uri) { 
            this.clusterUriList = new List<Uri> { uri };
        }
        
        public IList<Uri> GetCluster()
        {
            return clusterUriList;
        }
    }
}