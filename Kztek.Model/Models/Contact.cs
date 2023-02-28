using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
   public class Contact//funcname:Liên hệ
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(128)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Mã khách hàng")]
        public string CustomerId { get; set; }

        [Display(Name = "Thông tin khách hàng gửi lên")]
        public string Description { get; set; }//textarea

        [StringLength(50)]
        [Display(Name = "Ip Khách hàng")]
        [Required(ErrorMessage = "Nhập Ip khách hàng")]
        public string IPCustomer { get; set; }

        public DateTime DateCreated { get; set; }//hide
    }

    public class ContactView
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(128)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Mã khách hàng")]
        public string CustomerId { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }

        [Display(Name = "Thông tin khách hàng gửi lên")]
        public string Description { get; set; }//textarea

        [StringLength(50)]
        [Display(Name = "Ip Khách hàng")]
        [Required(ErrorMessage = "Nhập Ip khách hàng")]
        public string IPCustomer { get; set; }

        public DateTime DateCreated { get; set; }//hide

        public WebInfo Info { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }

    public class ContactCustom//funcname:Liên hệ
    {
        
        public string Id { get; set; }//key

       
        public string CustomerId { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }//textarea

     
        public string IPCustomer { get; set; }

        public DateTime DateCreated { get; set; }//hide



    }
}
