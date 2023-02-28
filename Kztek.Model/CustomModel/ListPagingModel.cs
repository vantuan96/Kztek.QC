using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class ListPagingModel
    {
        public string keyword { get; set; }
        public string fDate { get; set; }
        public string eDate { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string filter { get; set; }
    }
}
