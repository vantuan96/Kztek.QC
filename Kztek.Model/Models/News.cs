using Kztek.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
   public class News//funcname:Tin bài
    {
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }//key

        [StringLength(128)]
        [Display(Name = "Chọn danh mục")]
        public string NewsCategoryId { get; set; }//reftree-NewsCategory-Name

        [StringLength(500)]
        [Display(Name = "Tên bài viết")]
        [Required(ErrorMessage = "Nhập tiêu đề bài viết")]
        public string Name { get; set; }

        [StringLength(1000)]
        [Display(Name = "Tên bài viết không dấu")]
        public string NameUrl { get; set; }

        [StringLength(1000)]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }//textarea

        [Display(Name = "Nội dung")]
        public string Description { get; set; }//ckeditor

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
        public string CoverPath { get; set; }//image

        [Display(Name = "Thứ tự hiển thị ")]
        public int? TotalView { get; set; }

        [Display(Name = "Hot/Special ")]
        public int? NewsType { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }//hide

        [Display(Name = "Trạng thái kích hoạt")]
        public bool Active { get; set; }

        public string DetailPath { get; set; }//image

    }
    public class NewsView
    {
        public List<News> List { get; set; }

        public List<NewsCategory> ListCategory { get; set; }

        public string NewsCategoryName { get; set; } = "";

        public string NewsCategoryNameUrl { get; set; } = "";

        public string MetaTitle { get; set; } = "";

        public string MetaDesc { get; set; } = "";

        public string MetaKeyword { get; set; } = "";
        public PageModel<News> gridModel { get; set; }
    }
    public class ContentNews
    {
        public News objNews { get; set; }

        public List<News> ListHot { get; set; }
        public List<NewsCategory> ListCategory { get; set; }

        public string ServiceTitle { get; set; } = "";

        public string ServiceCategoryTitle { get; set; } = "";
        public string ServiceCategoryName { get; set; } = "";

        public string MetaTitle { get; set; } = "";

        public string MetaDesc { get; set; } = "";

        public string MetaKeyword { get; set; } = "";

        public string OGUrl { get; set; } = "";

        public string OGType { get; set; } = "article";

        public string OGTitle { get; set; } = "";

        public string OGDescription { get; set; } = "";

        public string OGImage { get; set; } = "";
    }
}
