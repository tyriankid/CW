namespace Hidistro.Entities.Orders
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;


    public class FilialeStoreListQuery : Pagination
    {
        public string fgsId { get; set; }

        public string StoreName { get; set; }

        public string AccountALLHere { get; set; }

        public string UserId { get; set; }

        public string UserIds { get; set; }

        public string IsCheck { get; set; }
    }
}


