using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Commodities
{
    public class UserDialogInfoQuery : Pagination
    {
        public int FQUserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
