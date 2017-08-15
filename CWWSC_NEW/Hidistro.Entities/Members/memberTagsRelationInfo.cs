using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hidistro.Entities.Members
{
    public class memberTagsRelationInfo
    {
        public int MtID { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }
        public int TagID { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
