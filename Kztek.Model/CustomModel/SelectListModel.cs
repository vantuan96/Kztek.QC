using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel
{
    public class SelectListModel
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
    }

    public class SelectListModel2
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
        public string ItemOtherValue { get; set; }
    }

    public class SelectListModel3
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
        public int ItemOtherValue { get; set; }
    }

    public class SelectListModel4
    {
        public string DriverLicense { get; set; }
        public string DriverLicenseRank { get; set; }
        public string DateActive { get; set; }
        public string DateExpire { get; set; }
    }
    public class SelectListModelTree
    {
        public string ItemValue { get; set; }
        public string ItemText { get; set; }
        public string ParentValue { get; set; }//Cấp cha 
    }
}
