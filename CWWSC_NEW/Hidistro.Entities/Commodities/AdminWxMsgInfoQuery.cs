using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Commodities
{
    public class AdminWxMsgInfoQuery : Pagination
    {
        public int FQUserName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int JSUserId { get; set; }
        public string StoreName { get; set; }
    }
}
