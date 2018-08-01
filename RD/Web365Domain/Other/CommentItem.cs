using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web365Domain
{
    public class CommentItem : BaseModel
    {
        public string Name { get; set; }
        public string CateName { get; set; }
        public string Email { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Detail { get; set; }
        public int? CateJoinID { get; set; }
        public int? JoinID { get; set; }
        public bool? IsShow { get; set; }
        public int? CommentID { get; set; }
        public string Link { get; set; }
    }
}
