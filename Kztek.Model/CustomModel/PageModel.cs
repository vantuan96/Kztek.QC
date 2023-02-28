﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class PageModel<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int TotalPage { get; set; }

        public int TotalItem { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
