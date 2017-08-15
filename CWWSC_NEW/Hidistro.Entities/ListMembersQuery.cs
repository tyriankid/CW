namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;
    public class ListMembersQuery : Pagination
    {
        public string UserName { get; set; }
        public string MeberName { get; set; }
        public string StoreCode { get; set; }
        public int id  { get; set; }
    }
}
