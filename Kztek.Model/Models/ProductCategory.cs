using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
   public class ProductCategory//funcname : Nhóm sản phẩm
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(500)]
        [Display(Name = "Tên nhóm sản phẩm")]
        [Column(TypeName = "nvarchar")]
        [Required(ErrorMessage = "Nhập tên nhóm sản phẩm")]
        public string Name { get; set; }

        [StringLength(1000)]
        public string NameUrl { get; set; }

        [StringLength(128)]
        [Display(Name = "Cấp cha")]
        public string ParentId { get; set; }//reftree-MainMenu-Name

        [Display(Name = "Cấp độ")]
        public int? Depth { get; set; }//hide

        [StringLength(500)]
        [Display(Name = "Đệ quy cấp độ")]
        [Column(TypeName = "nvarchar")]
        public string BreadCrumb { get; set; }//hide

        [StringLength(500)]
        [Display(Name = "Mô tả tiêu đề thẻ meta")]      
        public string MetaTitle { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả nội dung thẻ meta")]
        public string MetaDesc { get; set; }//textarea

        [StringLength(500)]
        [Display(Name = "Mô tả từ khóa thẻ meta")]
        public string MetaKeywork { get; set; }

        [Display(Name = "Mô tả nhóm sản phẩm")]
        public string Description { get; set; }//ckeditor

        [StringLength(1000)]
        [Display(Name = "Hình ảnh Icon ")]
        public string IconPath { get; set; }//image

        [StringLength(1000)]
        [Display(Name = "Hình ảnh cover ")]
        public string CorverPath { get; set; }//image

        [Display(Name = "Thứ tự hiển thị ")]
        public int Ordering { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }

        
    }
}
