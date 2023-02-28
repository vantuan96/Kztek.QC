using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class WebInfo
    {
        [Key]
        public string Id { get; set; }

        [StringLength(500)]
        public string WebsiteName { get; set; }

        [StringLength(500)]
        public string MetaTitle { get; set; }

        [StringLength(1000)]
        public string MetaDesc { get; set; } //textarea

        [StringLength(1000)]
        public string MetaKeywork { get; set; } //textarea

        public int PagingFontEnd { get; set; }

        public int PagingBackEnd { get; set; }

        [StringLength(255)]
        public string EmailSystem { get; set; }

        [StringLength(255)]
        public string EmailPassSystem { get; set; }

        public string EmailBcc { get; set; } //textarea

        [StringLength(1000)]
        public string EmailCC { get; set; } //textarea

        [StringLength(1000)]
        public string LogoPath { get; set; }

        [StringLength(1000)]
        public string LogoUrl { get; set; }

        [StringLength(1000)]
        public string MasterToolCode { get; set; } //textarea

        [StringLength(1000)]
        public string AnalyticsCode { get; set; } //textarea

        [StringLength(1000)]
        public string LinkGoogleMap { get; set; } //textarea

        [StringLength(1000)]
        public string FaceBookCode { get; set; } //textarea

        [StringLength(1000)]
        public string FanpageUrl { get; set; }

        [Column(TypeName = "ntext")]
        public string CompanyInfo { get; set; } //ckeditor

        [StringLength(1000)]
        public string ChatCode { get; set; } //textarea
        public string Phone { get; set; }
        public string Footer { get; set; } //ckeditor
    }
}
