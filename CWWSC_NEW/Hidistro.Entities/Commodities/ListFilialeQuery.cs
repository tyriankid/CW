
namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;
    public class ListFilialeQuery : Pagination
    {
        public string fgsName { get; set; }
        public int id { get; set; }
    }
}
