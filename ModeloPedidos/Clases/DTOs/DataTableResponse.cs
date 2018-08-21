using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloPedidos.Clases
{
    public class DataTableResponse
    {
        public int draw { get; set; }
        public long recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object[] data { get; set; }
        public string error { get; set; }
    }

    public class DataBootGrid
    {
        public int current { get; set; }
        public int rowsCount { get; set; }
        public object[] rows { get; set; }
        public long total;
    }
}
