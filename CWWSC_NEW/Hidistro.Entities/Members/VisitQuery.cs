namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class VisitQuery : Pagination
    {
        public string EndTime { get; set; }

        public string StartTime { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public int DistributorsUserId { get; set; }
    }
}

