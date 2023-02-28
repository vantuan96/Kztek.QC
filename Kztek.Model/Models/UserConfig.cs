using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class UserConfig
    {
        [Key]
        public string Id { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string UserId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string StationDefaultId { get; set; }

        public Nullable<DateTime> DateCreated { get; set; }
    }
}
