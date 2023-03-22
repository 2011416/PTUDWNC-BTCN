using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts
{
    public class ICategoryQuery
    {
        public string Keyword { get; set; }
        public bool ShowOnMenu { get; set; }
    }
}
