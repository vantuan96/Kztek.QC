﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class Role
    {
        [Key]
        public string Id { get; set; }

        [StringLength(150)]
        public string RoleName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } //textarea

        public bool Active { get; set; }

        public bool IsDeleted { get; set; }
    }
}
