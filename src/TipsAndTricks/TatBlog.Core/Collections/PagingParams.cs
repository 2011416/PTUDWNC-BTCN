﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Collections
{
    public class PagingParams: IPagingParams
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }
        public string SortColumn { get; set; } = "Id";

        public string SortOrder { get; set; } = "ASC";
    }
}
