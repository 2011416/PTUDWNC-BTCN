using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime SubDated { get; set; }
        public DateTime UnsubDated { get; set; }
        public string CancelReason { get; set; }
        public bool Voluntary { get; set; }
        public string AdminNotes { get; set; }
    }
}
