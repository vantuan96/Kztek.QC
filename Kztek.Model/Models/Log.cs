using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class Log
    {
        [Key]
        public string Id { get; set; }

        [StringLength(150)]
        public string TableName { get; set; }

        [StringLength(150)]
        public string ColumnId { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        public bool isSuccess { get; set; } = false;

        public string Message { get; set; } //textarea

        [StringLength(50)]
        public string UserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public DateTime DateCreated { get; set; }//hide
    }
}
