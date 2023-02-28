using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
   public class Customer//funcname:Khách hàng
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(500)]
        [Display(Name = "Tên khách hàng")]
        [Required(ErrorMessage = "Nhập tên khách hàng")]
        public string FullName { get; set; }

        [StringLength(255)]
        [Display(Name = "Email")]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Số điện thoại đi động")]
        [Column(TypeName = "varchar")]
        public string Mobile { get; set; }

        [StringLength(50)]
        [Display(Name = "Số điện thoại")]
        [Column(TypeName = "varchar")]
        public string Phone { get; set; }

        [StringLength(1000)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }//textarea

        [Display(Name = "Giới tính")]
        public bool Gender { get; set; }

        [StringLength(255)]
        [Display(Name = "Website")]
        [Column(TypeName = "varchar")]
        public string Website { get; set; }

        [StringLength(1000)]
        [Display(Name = "Ảnh đại diện")]
        public string Avartar { get; set; }//image

        [StringLength(50)]
        [Display(Name = "Tỉnh thành, vùng miền")]
        public string Country { get; set; }

        [Display(Name = "Mô tả ngắn về khách hàng")]
        public string Description { get; set; }//textarea

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }
    }
}
