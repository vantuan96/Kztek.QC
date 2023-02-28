using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class Trash
    {
        [Key]
        public string Id { get; set; }

        public string TableName { get; set; }

        public string ColumnId { get; set; }

        public string LogId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
