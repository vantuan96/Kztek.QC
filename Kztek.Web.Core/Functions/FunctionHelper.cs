using Kztek.Data;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using Kztek.Web.Core.Models;
using Kztek.Web.Core.Helpers;
using Kztek.Model.CustomModel;

namespace Kztek.Web.Core.Functions
{
    public class FunctionHelper
    {

        public static string Cnn = ConfigurationManager.ConnectionStrings["KztekEntities"].ConnectionString;

        public static List<object> TemplateStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "Image", ItemText = "Hình ảnh"},
                                         new  { ItemValue = "Template", ItemText = "Template"}
                                    };
            return list;
        }

        public static List<object> LinkTarget()
        {
            var list = new List<object> {
                                        new  { ItemValue = "_blank", ItemText = "_blank"},
                                         new  { ItemValue = "_self", ItemText = "_self"},
                                         new  { ItemValue = "_parent", ItemText = "_parent"},
                                         new  { ItemValue = "_top", ItemText = "_top"}
                                    };
            return list;
        }

        public static List<SelectListModel> LogActions()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "Create", ItemText = "Thêm mới"},
                                         new SelectListModel { ItemValue = "Update", ItemText = "Sửa"},
                                         new SelectListModel { ItemValue = "Delete", ItemText = "Xóa"}
                                    };
            return list;
        }
        public static List<object> PayStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Đã thanh toán"},
                                         new  { ItemValue = "2", ItemText = "Chưa thanh toán"}
                                    };
            return list;
        }

        public static List<object> PayStatus1()
        {
            var list = new List<object> {
                                        new  { ItemValue = "", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "true", ItemText = "Đã thanh toán"},
                                         new  { ItemValue = "false", ItemText = "Chưa thanh toán"}
                                    };
            return list;
        }
        public static List<SelectListModel> TypeComment()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Thường"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Hot"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Nổi bật"}
                                    };
            return list;
        }
        public static List<SelectListModel> TypeNews()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Tin thường"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Tin hot"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Tin nổi bật"}
                                    };
            return list;
        }

        public static List<SelectListModel> MenuTypes()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "DM tin tức"},
                                         new SelectListModel { ItemValue = "1", ItemText = "DM sản phẩm"},
                                         new SelectListModel { ItemValue = "2", ItemText = "DM dịch vụ"}
                                    };
            return list;
        }

        public static List<object> FinishStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn tiến độ --"},
                                         new  { ItemValue = "1", ItemText = "Đã hoàn thành"},
                                         new  { ItemValue = "2", ItemText = "Chưa hoàn thành"}
                                    };
            return list;
        }

        public static List<object> GetProductStatusList()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "hethang", ItemText = "Sản phẩm đang hết hàng"},
                                         new  { ItemValue = "dangnhap", ItemText = "Sản phẩm đang nhập hàng"},
                                    };
            return list;
        }

        public static List<object> ActiveContract()
        {
            var list = new List<object> {
                new { ItemValue = "True", ItemText = "Hiệu lực" },
                new { ItemValue = "False", ItemText = "Hết hiệu lực" }
            };
            return list;
        }
        public static List<object> ActiveStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "", ItemText = "-- Lựa chọn trạng thái --"},
                                         new  { ItemValue = "True", ItemText = "Kích hoạt"},
                                         new  { ItemValue = "False", ItemText = "Chưa kích hoạt"}
                                    };
            return list;
        }


        public static List<object> MenuType()
        {
            var list = new List<object> {
                                        new  { ItemValue = "1", ItemText = "Menu"},
                                         new  { ItemValue = "2", ItemText = "Function"}
                                    };
            return list;
        }
        public static List<object> Position()
        {
            return new List<object>     {
                                            new  { ItemValue = "0", ItemText = "-- Chọn vị trí --"},
                                            new  { ItemValue = "Top", ItemText = "Trên"},
                                            new  { ItemValue = "Bottom", ItemText = "Dưới"},
                                            new  { ItemValue = "Left", ItemText = "Trái"},
                                            new  { ItemValue = "Right", ItemText = "Phải"},
                                            new  { ItemValue = "Center", ItemText = "Giữa"},
                                            new  { ItemValue = "LeftBody", ItemText = "Lề trái"},
                                            new  { ItemValue = "RightBody", ItemText = "Lề phải"},
                                            new  { ItemValue = "Popup", ItemText = "PopUp"},
                                        };
        }

        public static List<object> Page()
        {
            return new List<object>     {
                                            new  { ItemValue = "All", ItemText = "-- Tất cả --"},
                                            new  { ItemValue = "Home", ItemText = "Trang chủ"},
                                            new  { ItemValue = "Product", ItemText = "Sản phẩm"},
                                            new  { ItemValue = "News", ItemText = "Tin tức"},
                                            new  { ItemValue = "Service", ItemText = "Dịch vụ"},
                                            new  { ItemValue = "Contact", ItemText = "Trang Liên hệ"}
                                        };
        }
        public static List<SelectListModel> TargetMenu()
        {
            return new List<SelectListModel>     {
                new  SelectListModel{ ItemValue = "0", ItemText = "--Mặc định--"},
                                            new  SelectListModel{ ItemValue = "1", ItemText = "Phần mềm"},
                                            new  SelectListModel{ ItemValue = "2", ItemText = "Thiết bị"},
                                            new  SelectListModel{ ItemValue = "3", ItemText = "Giải pháp"},
                                            new  SelectListModel{ ItemValue = "4", ItemText = "Tin tức"}

                                        };
        }
        public static List<SelectListModel> NewsCategoryType()
        {
            return new List<SelectListModel>     {
                                            new  SelectListModel{ ItemValue = "0", ItemText = "Phần mềm"},
                                            new  SelectListModel{ ItemValue = "1", ItemText = "Thiết bị"},
                                            new  SelectListModel{ ItemValue = "2", ItemText = "Giải pháp"},
                                            new  SelectListModel{ ItemValue = "3", ItemText = "Tin tức"}

                                        };
        }
        public static List<SelectListModel> MediaType()
        {
            return new List<SelectListModel>     {
                                            new  SelectListModel{ ItemValue = "0", ItemText = "Hình ảnh"},
                                            new  SelectListModel{ ItemValue = "1", ItemText = "Video"},
                                            new  SelectListModel{ ItemValue = "2", ItemText = "File"}
                                           
                                        };
        }
        public static List<SelectListModel> PageModel()
        {
            return new List<SelectListModel>     {
                                            new  SelectListModel{ ItemValue = "All", ItemText = "Tất cả"},
                                            new  SelectListModel{ ItemValue = "Home", ItemText = "Trang chủ"},
                                            new  SelectListModel{ ItemValue = "Product", ItemText = "Sản phẩm"},
                                            new  SelectListModel{ ItemValue = "News", ItemText = "Tin tức"},
                                            new  SelectListModel{ ItemValue = "Service", ItemText = "Dịch vụ"},
                                            new  SelectListModel{ ItemValue = "Contact", ItemText = "Trang Liên hệ"}
                                        };
        }
public static List<SelectListModel> PositionModel()
        {
            return new List<SelectListModel>     {
                                            new  SelectListModel{ ItemValue = "Top", ItemText = "Trên"},
                                            new  SelectListModel{ ItemValue = "Bottom", ItemText = "Dưới"},
                                            new  SelectListModel{ ItemValue = "Left", ItemText = "Trái"},
                                            new  SelectListModel{ ItemValue = "Right", ItemText = "Phải"},
                                            new  SelectListModel{ ItemValue = "Center", ItemText = "Giữa"},
                                            new  SelectListModel{ ItemValue = "LeftBody", ItemText = "Lề trái"},
                                            new  SelectListModel{ ItemValue = "RightBody", ItemText = "Lề phải"},
                                            new  SelectListModel{ ItemValue = "Popup", ItemText = "PopUp"},
                                        };
        }

        public static List<object> BannerStatus()
        {
            return new List<object>     {
                                            new  { ItemValue = "0", ItemText = "-- Lựa chọn hành động --"},
                                            new  { ItemValue = "run", ItemText = "Hoạt động"},
                                            new  { ItemValue = "pause", ItemText = "Tạm dừng"},
                                            new  { ItemValue = "stop", ItemText = "Hết hạn"}
                                        };
        }
       

        public static List<object> StockStatus()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn tình trạng --"},
                                         new  { ItemValue = "1", ItemText = "Còn hàng"},
                                         new  { ItemValue = "2", ItemText = "Hết hàng"},
                                         new  { ItemValue = "3", ItemText = "Đang về"}
                                    };
            return list;
        }

        public static List<object> DeleteStatus()
        {
            var list = new List<object> {
                                         new  { ItemValue = "False", ItemText = "Chưa xóa"},
                                         new  { ItemValue = "True", ItemText = "Đã xóa"}
                                    };
            return list;
        }

        public static List<object> GetProductOrder()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Giá tăng dần"},
                                         new  { ItemValue = "2", ItemText = "Giá giảm dần"},
                                         new  { ItemValue = "3", ItemText = "Tên từ A - Z"},
                                         new  { ItemValue = "4", ItemText = "Tên từ Z - A" },
                                    };
            return list;
        }

        public static List<object> Status()
        {
            var list = new List<object> {
                                        new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Sử dụng"},
                                         new  { ItemValue = "2", ItemText = "Không sử dụng"},
                                    };
            return list;
        }

        public static List<SelectListModel> DateType()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Ngày"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Tháng"}

                                    };
            return list;
        }

        public static List<object> HubList()
        {
            var list = new List<object> {
                                        new  { ItemValue = "", ItemText = "-- Lựa chọn loại--"},
                                         new  { ItemValue = "1", ItemText = "Trái"},
                                         new  { ItemValue = "2", ItemText = "Phải"},
                                    };
            return list;
        }

        public static List<object> SelectOptionFilterOrder()
        {
            return new List<object> {
                                         new  { ItemValue = "", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "0", ItemText = "Tất cả"},
                                         new  { ItemValue = "today", ItemText = "Hôm nay"},
                                         new  { ItemValue = "yesterday", ItemText = "Hôm qua"},
                                         new  { ItemValue = "Lastweek", ItemText = "7 ngày trước"},
                                         new  { ItemValue = "LastMonth", ItemText = "Tháng này"},
                                    };
        }
        public static List<object> FillterTime()
        {
            return new List<object> {
                                         new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "1", ItemText = "Hôm nay"},
                                         new  { ItemValue = "2", ItemText = "Hôm qua"},
                                         new  { ItemValue = "3", ItemText = "7 ngày trước"},
                                         new  { ItemValue = "4", ItemText = "Tháng này"},
                                    };
        }


        public static List<object> ReaderTypes()
        {
            return new List<object>
            {
                new {ItemValue = "0", ItemText = "1"},
                new {ItemValue = "1", ItemText = "2"}
            };
        }

        //public static List<statusOrderModel> StatusOrder()
        //{
        //    return new List<statusOrderModel> {
        //                                 new  statusOrderModel{ ItemValue = "0", ItemText = "-- Lựa chọn trạng thái hóa đơn --"},
        //                                 new  statusOrderModel{ ItemValue = "1", ItemText = "Chưa xác nhận"},
        //                                 new  statusOrderModel{ ItemValue = "2", ItemText = "Đã xác nhận"},
        //                                 new  statusOrderModel{ ItemValue = "3", ItemText = "Hủy đơn"},
        //                                 new  statusOrderModel{ ItemValue = "4", ItemText = "Hoãn đơn"},
        //                                 new  statusOrderModel{ ItemValue = "5", ItemText = "Hoàn thành"}
        //                            };
        //}

        public static string GetNameByStatus(string Name)
        {
            string str = "";
            switch (Name)
            {
                case "Home":
                    str = "Trang chủ";
                    break;

                case "Category":
                    str = "Trang Danh mục";
                    break;

                case "ProductDetail":
                    str = "Trang chi tiết";
                    break;

                case "Promotion":
                    str = "Trang khuyến mại";
                    break;

                case "Search":
                    str = "Trang tìm kiếm";
                    break;

                case "Provider":
                    str = "Trang nhà cung cấp";
                    break;

                case "Top":
                    str = "Trên";
                    break;

                case "Left":
                    str = "Trái";
                    break;

                case "Right":
                    str = "Phải";
                    break;

                case "Bottom":
                    str = "Dưới";
                    break;

                case "Center":
                    str = "Giữa";
                    break;

                case "Slide":
                    str = "Slide";
                    break;

                case "run":
                    str = "Hoạt động";
                    break;

                case "pause":
                    str = "Tạm dừng";
                    break;

                case "stop":
                    str = "Hết hạn";
                    break;

                case "choduyet":
                    str = "Chờ duyệt";
                    break;

                case "kichhoat":
                    str = "Kích hoạt";
                    break;

                case "color":
                    str = "mầu sắc";
                    break;

                case "size":
                    str = "Kích thước";
                    break;

                case "xuly":
                    str = "Đang xử lý";
                    break;

                case "finish":
                    str = "Đã xong";
                    break;

                default:
                    str = "";
                    break;
            }

            return str;
        }

        public static string GenTextStatus(DateTime beginDate, DateTime endDate)
        {
            string str = string.Empty;

            if (DateTime.Now.Date >= beginDate.Date && DateTime.Now <= endDate.Date)
            {
                str = "Hoạt động";
            }
            else
            {
                str = "<span class='textStatusRed'>Hết thời gian</span>";
            }
            return str;
        }

        public static string SplitContent(string value, int num)
        {
            string result = string.Empty;
            int start = 0;
            if (value.Length > num)
            {
                result = value.Substring(0, num);
                start = result.LastIndexOf(' ') - 1;
                result = result.Substring(0, start + 1);
            }
            else
            {
                result = value;
            }
            return result;
        }

        public static List<object> TimeApp()
        {
            var list = new List<object> {
                                         new  { ItemValue = "0", ItemText = "-- Lựa chọn --"},
                                         new  { ItemValue = "7", ItemText = "7 ngày"},
                                         new  { ItemValue = "14", ItemText = "14 ngày"},
                                         new  { ItemValue = "30", ItemText = "30 ngày"},
                                         new  { ItemValue = "60", ItemText = "60 ngày"},
                                         new  { ItemValue = "90", ItemText = "90 ngày"},
                                         new  { ItemValue = "120", ItemText = "120 ngày"},
                                         new  { ItemValue = "150", ItemText = "150 ngày"}
                                    };
            return list;
        }

        public static List<object> MenuOptions()
        {
            return new List<object> {
                                         new  { ItemValue = "0", ItemText = "Menu"},
                                         new  { ItemValue = "1", ItemText = "Function"},
                                    };
        }


        public static List<object> LaneTypes()
        {
            return new List<object>
            {
                new  { ItemValue = "", ItemText = "-- Chọn loại"},
                new  { ItemValue = "0", ItemText = "0. Vào"},
                new  { ItemValue = "1", ItemText = "1. Ra"}
                //new  { ItemValue = "2", ItemText = "2. Vào-Ra"},
                //new  { ItemValue = "3", ItemText = "3. Vào-Vào"},
                //new  { ItemValue = "4", ItemText = "4. Ra-Ra"},
                //new  { ItemValue = "5", ItemText = "5. Vào-Ra 2"},
            };
        }

        public static List<object> CheckPlateLevelOuts()
        {
            return new List<object>
            {
                new  { ItemValue = "1", ItemText = "So sánh >=4 ký tự"},
                new  { ItemValue = "2", ItemText = "So sánh tất cả ký tự"},
                new  { ItemValue = "0", ItemText = "Không kiểm tra"}
            };
        }

        public static List<SelectListModel> DayOfWeek()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "Mon", ItemText = "Thứ 2"},
                                         new SelectListModel { ItemValue = "Tue", ItemText = "Thứ 3"},
                                         new SelectListModel { ItemValue = "Wed", ItemText = "Thứ 4"},
                                         new SelectListModel { ItemValue = "Thu", ItemText = "Thứ 5"},
                                         new SelectListModel { ItemValue = "Fri", ItemText = "Thứ 6"},
                                         new SelectListModel { ItemValue = "Sat", ItemText = "Thứ 7"},
                                         new SelectListModel { ItemValue = "Sun", ItemText = "Chủ nhật"},
                                    };
            return list;
        }

        public static List<SelectListModel> RunType()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Theo lượt"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Theo tháng"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Theo quý"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Theo năm"}
                                    };
            return list;
        }

        public static List<object> PayStatus2()
        {
            var list = new List<object> {
                                         new  { ItemValue = "true", ItemText = "Đã thanh toán"},
                                         new  { ItemValue = "false", ItemText = "Chưa thanh toán"}
                                    };
            return list;
        }


        //Hàm đệ quy hiển thị cho List data
        public static List<SelectListModelTree> addParentItemToDDLtree(List<SelectListModelTree> list)
        {
            var _list = new List<SelectListModelTree>();
            if (list.Any())
            {
                var _lstParent = list.Where(c => c.ParentValue == "0").ToList();
                if (_lstParent != null && _lstParent.Any())
                {
                    foreach (var item in _lstParent)
                    {
                        //Thêm cấp cha
                        _list.Add(new SelectListModelTree { ItemValue = item.ItemValue, ItemText = item.ItemText, ParentValue = item.ParentValue });
                        //Tìm và thêm cấp con
                        var _lstChild = list.Where(c => c.ParentValue == item.ItemValue).ToList();
                        if (_lstChild.Any())
                        {
                            var depth = 1;
                            addChildItemToDDLtree(_list, _lstChild, list, depth);
                        }
                    }
                }
                else //Nếu ko có parentId thì hiển thị 1 cấp
                {
                    foreach (var item in list)
                    {
                        //Thêm cấp cha
                        _list.Add(new SelectListModelTree { ItemValue = item.ItemValue, ItemText = item.ItemText, ParentValue = item.ParentValue });
                        //Tìm và thêm cấp con
                        var _lstChild = list.Where(c => c.ParentValue == item.ItemValue).ToList();
                        if (_lstChild != null && _lstChild.Any())
                        {
                            var depth = 1;
                            addChildItemToDDLtree(_list, _lstChild, list, depth);
                        }
                    }
                }
            }

            return _list;
        }

        private static void addChildItemToDDLtree(List<SelectListModelTree> listAdd, List<SelectListModelTree> listChild, List<SelectListModelTree> list, int depth)
        {
            if (listChild.Any())
            {
                foreach (var item in listChild)
                {
                    var _name = "&#9502;" + item.ItemText;
                    var _space = "";
                    for (int i = 1; i <= depth; i++)
                    {
                        _space += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    }
                    _name = _space + _name;

                    listAdd.Add(new SelectListModelTree { ItemValue = item.ItemValue, ItemText = HttpUtility.HtmlDecode(_name), ParentValue = item.ParentValue });
                    //Tìm và thêm cấp con
                    var _lstChild = list.Where(c => c.ParentValue == item.ItemValue).ToList();
                    if (_lstChild.Any())
                    {
                        //Gọi lại hàm
                        var cDepth = depth + 1;
                        addChildItemToDDLtree(listAdd, _lstChild, list, cDepth);
                    }
                }
                depth++;
            }
        }


        /// <summary>
        /// Lay danh sach tinh thanh quan huyen tu file Country.xml
        /// </summary>
        /// <returns></returns>
        //public static List<DistrictModel> GetProvinceList()
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/Country.xml"));

        //    var regions = from region in xmlDoc.Descendants("Rows")
        //                  where int.Parse(region.Element("ParentID").Value) == 0
        //                  select new
        //                  {
        //                      TinhThanhID = region.Element("TinhThanhID").Value,
        //                      Name = region.Element("Name").Value,
        //                      ParentID = region.Element("ParentID").Value,
        //                  };

        //    var tinhThanhList = new List<DistrictModel>();

        //    foreach (var region in regions)
        //    {
        //        tinhThanhList.Add(new DistrictModel()
        //        {
        //            DistrictId = region.TinhThanhID,
        //            DistrictName = region.Name
        //        });
        //    }
        //    return tinhThanhList;
        //}

        /// <summary>
        /// Lay danh sach quan huyen tu file Country.xml
        /// </summary>
        /// <returns></returns>
        //public static List<DistrictModel> GetDistrictList(int tinhThanhId)
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/Country.xml"));

        //    var regions = from region in xmlDoc.Descendants("Rows")
        //                  where int.Parse(region.Element("ParentID").Value) == tinhThanhId
        //                  select new
        //                  {
        //                      TinhThanhID = region.Element("TinhThanhID").Value,
        //                      Name = region.Element("Name").Value,
        //                      ParentID = region.Element("ParentID").Value,
        //                  };

        //    var quanHuyenList = new List<DistrictModel>();

        //    foreach (var region in regions)
        //    {
        //        quanHuyenList.Add(new DistrictModel()
        //        {
        //            DistrictId = region.TinhThanhID,
        //            DistrictName = region.Name
        //        });
        //    }
        //    return quanHuyenList;
        //}

        //public static List<CountryModel> GetCountry()
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/New/country.xml"));

        //    var regions = from region in xmlDoc.Descendants("country")
        //                  select new
        //                  {
        //                      CountryId = region.Element("country_id").Value,
        //                      CountryName = region.Element("name").Value,
        //                      CountryCode = region.Element("iso_code").Value,
        //                  };

        //    var CountryList = new List<CountryModel>();

        //    foreach (var region in regions)
        //    {
        //        CountryList.Add(new CountryModel()
        //        {
        //            CountryId = region.CountryId,
        //            CountryName = region.CountryName,
        //            CountryCode = region.CountryCode
        //        });
        //    }
        //    return CountryList;
        //}

        //public static List<StateModel> GetStateByCountry(string countryId)
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/New/state.xml"));

        //    var regions = from region in xmlDoc.Descendants("state_province")
        //                  where Convert.ToString(region.Element("country_id").Value).Equals(countryId)
        //                  select new
        //                  {
        //                      StateAbbreviation = region.Element("abbreviation").Value,
        //                      StateName = region.Element("name").Value,
        //                      CountryId = region.Element("country_id").Value,
        //                  };

        //    var StateList = new List<StateModel>();

        //    foreach (var region in regions)
        //    {
        //        StateList.Add(new StateModel()
        //        {
        //            StateAbbreviation = region.StateAbbreviation,
        //            StateName = region.StateName,
        //            CountryId = region.CountryId
        //        });
        //    }
        //    return StateList;
        //}

        //public static List<StateModel> GetState()
        //{
        //    XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Templates/New/state.xml"));

        //    var regions = from region in xmlDoc.Descendants("state_province")
        //                  select new
        //                  {
        //                      StateAbbreviation = region.Element("abbreviation").Value,
        //                      StateName = region.Element("name").Value,
        //                      CountryId = region.Element("country_id").Value,
        //                  };

        //    var StateList = new List<StateModel>();

        //    foreach (var region in regions)
        //    {
        //        StateList.Add(new StateModel()
        //        {
        //            StateAbbreviation = region.StateAbbreviation,
        //            StateName = region.StateName,
        //            CountryId = region.CountryId
        //        });
        //    }
        //    return StateList;
        //}

        public static bool emailIsValid(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //public static bool SendMailRegisterSucccess(string cusName, string cusMail, ConfigSys info)
        //{
        //    try
        //    {
        //        var mailTo = cusMail;
        //        var mailCC = info.EmailCc; //System.Configuration.ConfigurationManager.AppSettings["CCOrderMail"] + ";" + System.Configuration.ConfigurationManager.AppSettings["ReceiveOrderMail"];
        //        var subject = "Kích hoạt tài khoản mới đăng ký " + HttpContext.Current.Request.Url.Host;

        //        //reading email template
        //        var content = ReadMailTemplate("~/Templates/Email/RegisterMailTemplate.xml");
        //        string emailContent = string.Format(content, cusName, cusMail);
        //        //SmtpEmailSender.SendSystem(mailFrom, mailTo, mailCC, subject, emailContent);
        //        return SmtpEmailSender.Send(mailTo, mailCC, subject, emailContent, info);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Doc file xml tra ve du lieu cua cot chi dinh
        /// </summary>
        /// <param name="path"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        //public static List<HtmlStatic> ReadXmlHtmlStatics()
        //{
        //    var xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Areas/Admin/htmlStatic.xml"));

        //    var regions = from region in xmlDoc.Descendants("Rows")
        //                  //where int.Parse(region.Element("ParentID").Value) == 0
        //                  select new HtmlStatic
        //                  {
        //                      Id = region.Element("Id").Value,
        //                      Title = region.Element("Title").Value,
        //                      Desc = region.Element("Desc").Value,
        //                      type = region.Element("Type").Value
        //                  };
        //    return regions.ToList();
        //}
        //public static HtmlStatic ReadXmlHtmlStaticsById(string id)
        //{
        //    var xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Areas/Admin/htmlStatic.xml"));

        //    var htmlStatics = from htmlStatic in xmlDoc.Descendants("Rows")
        //                      where htmlStatic.Element("Id").Value == id
        //                      orderby htmlStatic.Element("Type").Value ascending
        //                      select new HtmlStatic
        //                      {
        //                          Id = htmlStatic.Element("Id").Value,
        //                          Title = htmlStatic.Element("Title").Value,
        //                          Desc = htmlStatic.Element("Desc").Value,
        //                          type = htmlStatic.Element("Type").Value
        //                      };
        //    return htmlStatics.FirstOrDefault();
        //}

        /// <summary>
        /// Ham thay the tham so Url dung cho bo loc
        /// </summary>
        /// <param name="path">Url hien tai dang day du tham so</param>
        /// <param name="paramName">ten tham so</param>
        /// <param name="addOrRemove">them hay xoa tham so khoi Url hien tai - True: them moi, False: xoa di</param>
        /// <returns>do-so-sinh?price=</returns>
        public static string addUrlFilter(string path, string paramName, bool addOrRemove)
        {
            var _url = "";
            var arrUrl = path.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            // neu co tham so khac
            if (arrUrl.Length > 1)
            {
                //Replace('?', '&');
                if (arrUrl.Length > 2)
                    for (var i = 2; i <= arrUrl.Length - 1; i++)
                    {
                        arrUrl[1] += "&" + arrUrl[i];
                    }
                var urlParam = arrUrl[1].Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                if (urlParam.Length <= 1) // co duy nhat 1 tham so
                {
                    // neu tham so duy nhat nay giong voi tham so can thay the thi giu nguyen ten nhung bo di gia tri cua tham so
                    if (urlParam[0].Contains(paramName))
                    {
                        if (addOrRemove)
                            _url = arrUrl[0] + "?" + paramName + "=";
                        else
                            _url = arrUrl[0];
                    }
                    else
                    {
                        if (addOrRemove)
                            _url = arrUrl[0] + "?" + arrUrl[1] + "&" + paramName + "=";
                        else
                            _url = arrUrl[0] + "?" + arrUrl[1];
                    }
                }
                else // neu co nhieu hon 1 tham so
                {
                    _url = arrUrl[0] + "?";
                    // kiem tra xem tham so co chua
                    //_url = urlParam.Where(s => !s.Contains(paramName)).Aggregate(_url, (current, s) => current + (s + "&"));
                    foreach (var s in urlParam)
                    {
                        // neu tham so da co san
                        if (!s.Contains(paramName) && s.Contains("="))
                            _url += s + "&";
                    }
                    // them moi param hay xoa di
                    if (addOrRemove)
                        _url += paramName + "=";
                    else
                        _url = _url.Substring(0, _url.Length - 1);
                }
            }
            else // neu khong co tham so nao
            {
                if (addOrRemove)
                    _url = arrUrl[0] + "?" + paramName + "=";
                else
                    _url = arrUrl[0];
            }
            return _url;
        }

        //public static string CovertImgtoBase64(string imagepath)
        //{
        //    try
        //    {
        //        using (Image image = Image.FromFile(imagepath))
        //        {
        //            using (MemoryStream m = new MemoryStream())
        //            {
        //                image.Save(m, image.RawFormat);
        //                byte[] imageBytes = m.ToArray();
        //                string base64String = Convert.ToBase64String(imageBytes);
        //                return base64String;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message;
        //        throw ex;
        //    }
        //}

        public static string ConvertImgFileUploadToBase64(HttpPostedFileBase FilesUpload)
        {
            string ImageUrl = "";

            Byte[] bytes = CovertFileToByte(FilesUpload);

            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

            ImageUrl = "data:image/png;base64," + base64String;

            return ImageUrl;
        }

        public static byte[] CovertFileToByte(HttpPostedFileBase FilesUpload)
        {
            BinaryReader br = new BinaryReader(FilesUpload.InputStream);
            byte[] buffer = br.ReadBytes(FilesUpload.ContentLength);

            return buffer;
        }

        public static string GetRegisterFromUrl(string url)
        {
            string strRegister = string.Empty;
            string[] strPath = url.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (strPath.Any())
            {
                strRegister = strPath[0].ToString();
            }
            return strRegister;
        }

        /// <summary>
        /// ex: Thứ hai, 6/2/2017 | 16:10
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        public static string showDayVN(DateTime _dt)
        {
            var strRtn = "{0}, {1} | {2}";
            var thu = "";
            if (_dt != null || _dt != DateTime.MinValue)
            {
                //Friday = 5,
                //Monday = 1,
                //Saturday = 6,
                //Sunday = 0,
                //Thursday = 4,
                //Tuesday = 2,
                //Wednesday = 3
                switch (Convert.ToInt32(_dt.DayOfWeek))
                {
                    case 0:
                        thu = "Chủ nhật";
                        break;

                    case 1:
                        thu = "Thứ hai";
                        break;

                    case 2:
                        thu = "Thứ ba";
                        break;

                    case 3:
                        thu = "Thứ tư";
                        break;

                    case 4:
                        thu = "Thứ năm";
                        break;

                    case 5:
                        thu = "Thứ sáu";
                        break;

                    case 6:
                        thu = "Thứ bẩy";
                        break;

                    default:
                        thu = "Chủ nhật";
                        break;
                }
            }

            return string.Format(strRtn, thu, _dt.ToString("dd/MM/yyyy"), _dt.ToString("hh:mm"));
        }


        public static void ClearCache(string name)
        {
            CacheLayer.Clear(name);
        }

        public static void ClearCache(string constField, string objId)
        {
            var formatRole = string.Format("{0}_{1}", constField, objId);
            CacheLayer.Clear(formatRole);
        }

        public static string GetCgiByCameraType(string CameraType, string FrameRate, string Resolution, string SDK)
        {
            string Cgi = "";
            switch (CameraType)
            {
                case "Panasonic i-Pro":
                    Cgi = "/cgi-bin/mjpeg?framerate=" + FrameRate + "&resolution=" + Resolution;
                    break;
                case "Axis":
                    Cgi = "/axis-cgi/mjpg/video.cgi?resolution = " + Resolution;
                    break;
                case "Sony":
                    Cgi = "/image";
                    break;
                case "Shany-Stream1":
                    Cgi = "/live/stream1.cgi";
                    break;
                case "Shany-Stream2":
                    Cgi = "/live/stream2.cgi";
                    break;
                case "Secus":
                    if (SDK == "FFMPEG")
                    {
                        Cgi = "/stream1";
                        break;
                    }
                    Cgi = "/cgi-bin/image/mjpeg.cgi";

                    break;
                case "Bosch":
                    if (SDK == "FFMPEG")
                    {
                        Cgi = "/rtsp_tunnel";
                        break;
                    }
                    Cgi = "/snap.jpg?";
                    break;
                case "Vantech":
                    Cgi = "Vantech";
                    break;
                case "SecusFFMPEG":
                    Cgi = "Secus";
                    break;
                default:

                    break;
            }
            return Cgi;
        }

        public static bool CheckInt(string input, ref int num)
        {
            int numArc;

            bool isInt = int.TryParse(input, out numArc);
            if (isInt)
            {
                num = numArc;
            }
            else
            {
                num = 0;
            }

            return isInt;
        }

        public static bool CheckFloat(string input, ref float num)
        {
            float numArc;

            bool isFloat = float.TryParse(input, out numArc);
            if (isFloat)
            {
                num = numArc;
            }
            else
            {
                num = 0;
            }

            return isFloat;
        }

        public static int GetTotalMonthsFrom(DateTime dt1, DateTime dt2)
        {
            DateTime earlyDate = (dt1 > dt2) ? dt2.Date : dt1.Date;
            DateTime lateDate = (dt1 > dt2) ? dt1.Date : dt2.Date;

            // Start with 1 month's difference and keep incrementing
            // until we overshoot the late date
            int monthsDiff = 1;
            while (earlyDate.AddMonths(monthsDiff) <= lateDate)
            {
                monthsDiff++;
            }

            return monthsDiff;
        }




        private static string[] ChuSo = new string[10] { " không", " một", " hai", " ba", " bốn", " năm", " sáu", " bảy", " tám", " chín" };
        private static string[] Tien = new string[6] { "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ" };
        // Hàm đọc số thành chữ
        public static string DocTienBangChu(long SoTien, string strTail)
        {
            int lan, i;
            long so;
            string KetQua = "", tmp = "";
            int[] ViTri = new int[6];
            if (SoTien < 0) return "Số tiền âm !";
            if (SoTien == 0) return "Không đồng !";
            if (SoTien > 0)
            {
                so = SoTien;
            }
            else
            {
                so = -SoTien;
            }
            //Kiểm tra số quá lớn
            if (SoTien > 8999999999999999)
            {
                SoTien = 0;
                return "";
            }
            ViTri[5] = (int)(so / 1000000000000000);
            so = so - long.Parse(ViTri[5].ToString()) * 1000000000000000;
            ViTri[4] = (int)(so / 1000000000000);
            so = so - long.Parse(ViTri[4].ToString()) * +1000000000000;
            ViTri[3] = (int)(so / 1000000000);
            so = so - long.Parse(ViTri[3].ToString()) * 1000000000;
            ViTri[2] = (int)(so / 1000000);
            ViTri[1] = (int)((so % 1000000) / 1000);
            ViTri[0] = (int)(so % 1000);
            if (ViTri[5] > 0)
            {
                lan = 5;
            }
            else if (ViTri[4] > 0)
            {
                lan = 4;
            }
            else if (ViTri[3] > 0)
            {
                lan = 3;
            }
            else if (ViTri[2] > 0)
            {
                lan = 2;
            }
            else if (ViTri[1] > 0)
            {
                lan = 1;
            }
            else
            {
                lan = 0;
            }
            for (i = lan; i >= 0; i--)
            {
                tmp = DocSo3ChuSo(ViTri[i]);
                KetQua += tmp;
                if (ViTri[i] != 0) KetQua += Tien[i];
                if ((i > 0) && (!string.IsNullOrEmpty(tmp))) KetQua += ",";//&& (!string.IsNullOrEmpty(tmp))
            }
            if (KetQua.Substring(KetQua.Length - 1, 1) == ",") KetQua = KetQua.Substring(0, KetQua.Length - 1);
            KetQua = KetQua.Trim() + strTail;
            return KetQua.Substring(0, 1).ToUpper() + KetQua.Substring(1);
        }
        // Hàm đọc số có 3 chữ số
        private static string DocSo3ChuSo(int baso)
        {
            int tram, chuc, donvi;
            string KetQua = "";
            tram = (int)(baso / 100);
            chuc = (int)((baso % 100) / 10);
            donvi = baso % 10;
            if ((tram == 0) && (chuc == 0) && (donvi == 0)) return "";
            if (tram != 0)
            {
                KetQua += ChuSo[tram] + " trăm";
                if ((chuc == 0) && (donvi != 0)) KetQua += " linh";
            }
            if ((chuc != 0) && (chuc != 1))
            {
                KetQua += ChuSo[chuc] + " mươi";
                if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh";
            }
            if (chuc == 1) KetQua += " mười";
            switch (donvi)
            {
                case 1:
                    if ((chuc != 0) && (chuc != 1))
                    {
                        KetQua += " mốt";
                    }
                    else
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
                case 5:
                    if (chuc == 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    else
                    {
                        KetQua += " lăm";
                    }
                    break;
                default:
                    if (donvi != 0)
                    {
                        KetQua += ChuSo[donvi];
                    }
                    break;
            }
            return KetQua;
        }

        public static string GetDateTimeExpire(DateTime dateFrom, DateTime dateTo)
        {
            string result = "";

            var dateTimeNow = DateTime.Today;
            if (dateFrom <= dateTimeNow)
            {
                if (dateTo >= dateTimeNow)
                {
                    result = "<span class='label label-success label-white middle'><i class='ace-icon fa fa-check-circle-o'></i>  Còn hạn </span>";
                }
                else
                {
                    result = "<span class='label label-danger label-white middle'><i class='ace-icon fa fa-ban'></i>  Hết hạn </span>";
                }
            }
            else
            {
                result = "<span class='label label-warning label-white middle'><i class='ace-icon fa fa-exclamation-circle'></i>  Chưa đến ngày </span>";
            }

            return result;
        }

        public static string GetDateTimeOut(string timeOutInEvent, string timeOutInCommand, int timeAlert, string dateTimeOut)
        {
            string result = "";

            var timeInEvent = Convert.ToDateTime(timeOutInEvent);
            var timeInCommand = Convert.ToDateTime(timeOutInCommand);

            var timeSpan = timeInEvent - timeInCommand;

            var t = timeSpan.TotalMinutes;

            //t > 0 là xe ra muộn, < 0 là xe ra sớm
            if (t > 0)
            {
                if (t > timeAlert)
                {
                    result = string.Format("<span style='color:orange'>{0}</span>", dateTimeOut);
                }
                else
                {
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                }
            }
            else
            {
                var newT = -t;
                if (newT > timeAlert)
                {
                    result = string.Format("<span style='color:red'>{0}</span>", dateTimeOut);
                }
                else
                {
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                }
            }

            return result;
        }

        public static string GetTimeOutStatus(string status = "", string dateTimeOut = "")
        {
            string result = "";

            // 0 = mặc định, 1 = ra sớm, 2 = ra muộn
            switch (status)
            {
                case "0":
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                    break;
                case "1":
                    result = string.Format("<span style='color:red'>{0}</span>", dateTimeOut);
                    break;
                case "2":
                    result = string.Format("<span style='color:orange'>{0}</span>", dateTimeOut);
                    break;
                case "3":
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                    break;
                default:
                    result = string.Format("<span style='color:black'>{0}</span>", dateTimeOut);
                    break;
            }

            return result;
        }

        public static void GetDateTimeOutStatus(string timeOutInEvent, string timeOutInCommand, int timeAlert, ref string result, ref string status)
        {
            var timeInEvent = Convert.ToDateTime(timeOutInEvent);
            var timeInCommand = Convert.ToDateTime(timeOutInCommand);

            var timeSpan = timeInEvent - timeInCommand;

            var t = timeSpan.TotalMinutes;
            if (t > 0)
            {
                if (t > timeAlert)
                {
                    result = t.ToString();

                    status = "2";
                }
                else
                {
                    result = t.ToString();

                    status = "3";
                }
            }
            else
            {
                var newT = -t;
                if (newT > timeAlert)
                {
                    result = newT.ToString();

                    status = "1";
                }
                else
                {
                    result = newT.ToString();

                    status = "3";
                }
            }
        }

        public void GetDateTimeOutStatus1(string timeOutInEvent, string timeOutInCommand, int timeAlert, ref string result, ref string status)
        {
            var timeInEvent = Convert.ToDateTime(timeOutInEvent);
            var timeInCommand = Convert.ToDateTime(timeOutInCommand);

            var timeSpan = timeInEvent - timeInCommand;

            var t = timeSpan.TotalMinutes;
            if (t > 0)
            {
                if (t > timeAlert)
                {
                    result = t.ToString();

                    status = "2";
                }
                else
                {
                    result = t.ToString();

                    status = "3";
                }
            }
            else
            {
                var newT = -t;
                if (newT > timeAlert)
                {
                    result = newT.ToString();

                    status = "1";
                }
                else
                {
                    result = newT.ToString();

                    status = "3";
                }
            }
        }

        /// <summary>
        /// 1 - sớm 30' 2- muộn 10' 3- đúng giờ
        /// </summary>
        /// <param name="realDate"></param>
        /// <param name="dateCommand"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static SelectListModel GetDateTimeCommandStatus(string realDate, string dateCommand, string timeOut)
        {
            var result = new SelectListModel();

            var real = Convert.ToDateTime(realDate);
            var command = Convert.ToDateTime(string.Format("{0} {1}", Convert.ToDateTime(dateCommand).ToString("dd/MM/yyyy"), timeOut));
            var timespan = (command - real).Minutes;

            result.ItemText = "3";
            result.ItemValue = timespan.ToString();

            if (timespan > 0 && timespan >= 30)
            {
                result.ItemText = "1";
            }
            else if (timespan < 0 && -timespan >= 10)
            {
                result.ItemText = "2";
            }

            return result;
        }

        public static string GetDateTimeExpire(DateTime dateTo)
        {
            string result = "";

            var dateTimeNow = DateTime.Today;
            if (dateTo >= dateTimeNow)
            {
                result = "<span class='label label-success label-white middle'><i class='ace-icon fa fa-check-circle-o'></i>  Còn hạn </span>";
            }
            else
            {
                result = "<span class='label label-danger label-white middle'><i class='ace-icon fa fa-ban'></i>  Hết hạn </span>";
            }

            return result;
        }

        public static bool DetectSqlInjection(string Text)
        {
            string CleanText = Text.ToUpper().Replace("/**/", " ").Replace("+", " ").Replace("  ", " ");

            string[] InjectionPatterns = {
                                     "VARCHAR",
                                     "EXEC",
                                     "DECLARE",
                                     "SELECT *",
                                     "SELECT PASSWORD",
                                     "SELECT USERNAME",
                                     "$_GET",
                                     "NULL OR",
                                     "UNION ALL SELECT",
                                     "WAITFOR DELAY",
                                     "SELECT pg_sleep",
                                     "SHOW TABLES FROM"
                                     };

            foreach (string Pattern in InjectionPatterns)
            {
                if (CleanText.Contains(Pattern))
                    return true;
            }

            return false;
        }
    }
}