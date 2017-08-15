namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;
    using System.Data;

    public class FilialeStatisticsQuery : Pagination
    {
        public string FilialeName { get; set; }

        public string UserId { get; set; }

        public string UserIds { get; set; }

        public string IsCheck { get; set; }

        public string id { get; set; }

        public string ids { get; set; }


        public DataTable DtFgs { get; set; }
    }
}
