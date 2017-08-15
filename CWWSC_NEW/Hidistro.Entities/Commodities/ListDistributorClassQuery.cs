using Hidistro.Core.Entities;
using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Commodities
{
    public class ListDistributorClassQuery : Pagination
    {
        public string  keyword { get; set; }
        public string RealName { get; set; }
        public string accountALLHere { get; set; }
        public string  states { get; set; }
        public int DisUserId { get; set; }

    }
}
