using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.DTO
{
    public class PostQuery : IPostQuery
    {
        public int AuthorId { get; set; } = -1;
        public int CategoryId { get; set; } = -1;

        public string CategorySlug { get; set; } = "";
        public string AuthorSlug { get; set; } = "";
        public string TagSlug { get; set; } = "";
        public int PostedYear { get; set; } = -1;
        public int PostedMonth { get; set; } = -1;
        public bool PublishedOnly { get; set; } = true;

        public bool NotPublished { get; set; } = false;
        public string Keyword { get; set; } = "";
        public int Year { get; set; }
        public int Month { get; set; }
        public string TitleSlug { get; set; }
    }
}
