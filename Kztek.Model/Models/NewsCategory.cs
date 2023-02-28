using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class NewsCategory//funcname:Danh mục tin
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(500)]
        [Display(Name = "Tên danh mục")]
        [Column(TypeName = "nvarchar")]
        [Required(ErrorMessage = "Nhập tên danh mục")]
        public string Name { get; set; }

        [StringLength(128)]
        [Display(Name = "Cấp cha")]
        [Column(TypeName = "varchar")]
        public string ParentId { get; set; }//reftree-MainMenu-Name

        [Display(Name = "Cấp độ")]
        public int? Depth { get; set; }//hide

        [StringLength(500)]
        [Display(Name = "Đệ quy cấp độ")]
        [Column(TypeName = "nvarchar")]
        public string BreadCrumb { get; set; }//hide

        [StringLength(500)]
        [Display(Name = "Tên không dấu")]
        [Column(TypeName = "varchar")]
        public string NameUrl { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả tiêu đề thẻ meta")]     
        public string MetaTitle { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả nội dung thẻ meta")]     
        public string MetaDesc { get; set; }//textarea

        [StringLength(500)]
        [Display(Name = "MetaKeywork")]
        public string MetaKeywork { get; set; }//textarea

        [Display(Name = "Mô tả")]
        public string Description { get; set; }//textarea

        [StringLength(1000)]
        [Display(Name = "Hình ảnh icon")]
        [Column(TypeName = "nvarchar")]
        public string IconPath { get; set; }//image

        [StringLength(1000)]
        [Display(Name = "Hình ảnh cover ")]
        [Column(TypeName = "nvarchar")]
        public string CoverPath { get; set; }//image

        [Display(Name = "Thứ tự hiển thị")]
        public int? Ordering { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }

        public string Type { get; set; }
    }
}
