using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class Product//funcname : Sản phẩm
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(500)]
        [Display(Name = "Tên sản phẩm")]
        [Column(TypeName = "nvarchar")]
        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Barcode { get; set; }

        [StringLength(128)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Nhóm sản phẩm")]
        public string ProductCategoryId { get; set; }

        [StringLength(1000)]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }//textarea

        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; }//ckeditor

        [StringLength(1000)]
        [Display(Name = "Tên sản phẩm không dấu")]
        public string NameUrl { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả tiêu đề thẻ meta")]      
        public string MetaTitle { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả nội dung thẻ meta")]   
        public string MetaDesc { get; set; }//textarea

        [StringLength(500)]
        [Display(Name = "Mô tả từ khóa thẻ meta")]    
        public string MetaKeywork { get; set; }

        [StringLength(1000)]
        [Display(Name = "Hình ảnh cover ")]
        public string CorverPath { get; set; }//image

        [Display(Name = "Giá sản phẩm")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "không đúng định dạng tiền tệ")]
        public decimal Price { get; set; }//maskmoney

        [Display(Name = "Giá khuyến mại")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "không đúng định dạng tiền tệ")]
        public decimal PricePromotion { get; set; }//maskmoney

        [Display(Name = "Số lượng còn")]
        public int? Quantity { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int? Ordering { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }
        public string SummaryHome { get; set; }
    }

    public class ProductView
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(500)]
        [Display(Name = "Tên sản phẩm")]
        [Column(TypeName = "nvarchar")]
        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Barcode { get; set; }

        [StringLength(128)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Nhóm sản phẩm")]
        public string ProductCategoryId { get; set; }

        [StringLength(1000)]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }//textarea

        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; }//ckeditor

        [StringLength(1000)]
        [Display(Name = "Tên sản phẩm không dấu")]
        public string NameUrl { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả tiêu đề thẻ meta")]
        [Column(TypeName = "varchar")]
        public string MetaTitle { get; set; }

        [StringLength(1000)]
        [Display(Name = "Mô tả nội dung thẻ meta")]
        [Column(TypeName = "varchar")]
        public string MetaDesc { get; set; }//textarea

        [StringLength(500)]
        [Display(Name = "Mô tả từ khóa thẻ meta")]
        [Column(TypeName = "varchar")]
        public string MetaKeywork { get; set; }

        [StringLength(1000)]
        [Display(Name = "Hình ảnh cover ")]
        public string CorverPath { get; set; }//image

        [Display(Name = "Giá sản phẩm")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Không đúng định dạng tiền tệ")]
        public string Price { get; set; }//maskmoney

        [Display(Name = "Giá khuyến mại")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Không đúng định dạng tiền tệ")]
        public string PricePromotion { get; set; }//maskmoney

        [Display(Name = "Số lượng còn")]
        public int? Quantity { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int? Ordering { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }
        public string SummaryHome { get; set; }
    }
}
