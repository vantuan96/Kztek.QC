using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
   public class CustomerComment //funcname:Bình luận của khách hàng
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(500)]
        [Display(Name = "Họ Tên khách hàng")]
        [Required(ErrorMessage = "Nhập họ tên khách hàng")]
        public string FullName { get; set; }

        [StringLength(500)]
        [Display(Name = "Ảnh đại diện khách hàng")]
        public string Avartar { get; set; }//image

        [StringLength(1000)]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }

        [Display(Name = "Mô tả ngắn về khách hàng")]
        public string Description { get; set; }//textarea

        [Display(Name = "Thứ tự hiển thị")]
        public int? Ordering { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }
        public string Type { get; set; }
    }
}
