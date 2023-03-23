﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.DTO
{
    public class AuthorQuery : IAuthorQuery
    {
        public string Keyword { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
