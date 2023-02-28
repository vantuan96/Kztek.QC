using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class MainMenu//funcname:Quản lý Menu
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key
        [StringLength(500)]
        [Display(Name = "Tên Menu")]
        [Required(ErrorMessage = "Nhập tên Menu")]
        public string Name { get; set; }
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string ParentId { get; set; }//reftree-MainMenu-Name
        public int? Depth { get; set; }//hide
        [StringLength(500)]
        [Column(TypeName = "varchar")]
        public string BreadCrumb { get; set; }//hide
        [StringLength(500)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Url hiển thị")]
        public string NameUrl { get; set; }
        [StringLength(1000)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Liên kết")]
        public string Url { get; set; }
        [StringLength(1000)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Icon")]
        public string IconPath { get; set; }//image
        [StringLength(1000)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Ảnh cover")]
        public string CoverPath { get; set; }//image

        [Column(TypeName = "varchar")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }//textarea
        [Display(Name = "Thứ tự")]
        public int Ordering { get; set; }
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Trang")]
        public string Page { get; set; }
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Vị trí")]
        public string Position { get; set; }
        [StringLength(10)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Loại Menu")]
        public string Target { get; set; }
        public DateTime DateCreated { get; set; }//hide
        public bool Active { get; set; }
    }
}
