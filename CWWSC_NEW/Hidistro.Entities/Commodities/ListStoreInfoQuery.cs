using Hidistro.Core.Entities;
using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Commodities
{
    public class ListStoreInfoQuery : Pagination
    {
        public string storeName { get; set; }

        public string fgsids { get; set; }
    }
}
