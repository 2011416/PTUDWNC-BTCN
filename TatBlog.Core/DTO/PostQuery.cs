using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public string AuthorId { get; set; }
        public string CategoryId { get; set; }
        public string Slug { get; set; }
        public string PostedDate { get; set; }
        public string Tags { get; set; }
        public IList<string> SelectedTag { get; set; }
        public IEnumerable<string> SelectedAuthor { get; set; }
        public IEnumerable<string> SelectedCategory { get; set; }

    }
}
