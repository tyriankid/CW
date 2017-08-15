using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Commodities
{
    public class DistributorListQuery : Pagination
    {
        public string keyword { get; set; }
        public double lng { get; set; }
        public double lat { get; set; }
        public int NearByValue { get; set; }
    }
}
