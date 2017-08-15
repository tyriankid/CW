﻿namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorsQuery : Pagination
    {
        public string CellPhone { get; set; }

        public DateTime EndTime { get; set; }

        public int GradeId { get; set; }

        public string MicroSignal { get; set; }

        public string RealName { get; set; }

        public string ReferralPath { get; set; }

        public int ReferralStatus { get; set; }

        public DateTime StartTime { get; set; }

        public string StoreName { get; set; }

        public string AccountAllHere { get; set; }

        public int UserId { get; set; }

        public int IsAgent { get; set; }

        public string AgentPath { get; set; }

        public int IsServiceStore { get; set; }

        public string StoreIds { get; set; }

        public int fgsId { get; set; }

    }
}

