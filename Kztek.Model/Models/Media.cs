using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
   public class Media//funcname:Quản lý đa phương tiện
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(500)]
        [Display(Name = "Tên Menu")]
        [Column(TypeName = "nvarchar")]
        [Required(ErrorMessage = "Nhập tên Banner hoặc video")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar")]
        [Display(Name = "Mô tả ngắn")]
        public string Description { get; set; }//textarea

        [StringLength(1000)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Mô tả trên thẻ ảnh")]
        public string Alt { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Link chuyển hướng")]
        public string Url { get; set; }

        [StringLength(1000)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Đường dẫn tới file gốc")]
        public string Path { get; set; }//upload

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Trang hiển thị")]
        public string Page { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Vị trí trên trang")]
        public string Position { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Loại Media")]
        public string MediaType { get; set; }

        [Display(Name = "Lượt xem")]
        public int? TotalView { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int? Ordering { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }
    }
}
