using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kztek.Web.Core.Extensions
{
    #region TableColumn  

    /// <summary>    
    /// Các thuộc tính và phương thức được sử dụng trong lớp TableBuilder.    
    /// </summary>    
    public interface ITableColumnInternal<TModel> where TModel : class
    {
        string ColumnTitle
        {
            get;
            set;
        }
        string ColumnWidth
        {
            get;
            set;
        }
        string ColumnClass
        {
            get;
            set;
        }
        string ColumnName
        {
            get;
            set;
        }
        string Evaluate(TModel model);
    }

    /// <summary>    
    /// Các thuộc tính và phương thức được người tiêu dùng sử dụng để định cấu hình TableColumn.
    /// </summary>    
    public interface ITableColumn
    {
        ITableColumn Title(string title);
        ITableColumn tdWidth(string w);
        ITableColumn tdClass(string css);
    }

    /// <summary>    
    /// Đại diện cho một cột trong một bảng.
    /// </summary>    
    /// <typeparam name="TModel">Lớp được hiển thị trong bảng.</typeparam>    
    /// <typeparam name="TProperty">Thuộc tính lớp được hiển thị trong cột.</typeparam>    
    public class TableColumn<TModel,
    TProperty> : ITableColumn,
    ITableColumnInternal<TModel> where TModel : class
    {
        /// <summary>    
        /// Column title to display in the table.    
        /// </summary>    
        public string ColumnTitle { get; set; }
        public string ColumnWidth { get; set; } //Gán độ rộng của cột
        public string ColumnClass { get; set; } //gán class css cho cột
        public string ColumnName { get; set; } // tên trường
        /// <summary>    
        /// Biểu thức lambda đã biên dịch để lấy giá trị thuộc tính từ một đối tượng mô hình.   
        /// </summary>    
        public Func<TModel, TProperty> CompiledExpression
        {
            get;
            set;
        }


        /// <summary>    
        /// Constructor.    
        /// </summary>    
        /// <param name="expression">Lambda expression identifying a property to be rendered.</param>    
        public TableColumn(Expression<Func<TModel, TProperty>> expression)
        {
            string propertyName = (expression.Body as MemberExpression).Member.Name;
            this.ColumnTitle = Regex.Replace(propertyName, "([a-z])([A-Z])", "$1 $2");
            this.ColumnName = propertyName;
            this.CompiledExpression = expression.Compile();
        }

        /// <summary>    
        /// Set the title for the column.    
        /// </summary>    
        /// <param name="title">Title for the column.</param>    
        /// <returns>Instance of a TableColumn.</returns>    
        public ITableColumn Title(string title)
        {
            this.ColumnTitle = title;
            return this;
        }

        /// <summary>    
        /// Get the property value from a model object.    
        /// </summary>    
        /// <param name="model">Model to get the property value from.</param>    
        /// <returns>Property value from the model.</returns>    
        public string Evaluate(TModel model)
        {
            var result = this.CompiledExpression(model);
            return result == null ? string.Empty : result.ToString();
        }

        public ITableColumn tdWidth(string w)
        {
            this.ColumnWidth = w;
            return this;
        }

        public ITableColumn tdClass(string css)
        {
            this.ColumnClass = css;
            return this;
        }
    }

    #endregion TableColumn
    #region ColumnBuilder  

    /// <summary>    
    /// Create instances of TableColumns.    
    /// </summary>    
    /// <typeparam name="TModel">Type of model to render in the table.</typeparam>    
    public class ColumnBuilder<TModel> where TModel : class
    {
        public TableBuilder<TModel> TableBuilder
        {
            get;
            set;
        }

        /// <summary>    
        /// Constructor.    
        /// </summary>    
        /// <param name="tableBuilder">Instance of a TableBuilder.</param>    
        public ColumnBuilder(TableBuilder<TModel> tableBuilder)
        {
            TableBuilder = tableBuilder;
        }

        /// <summary>    
        /// Add lambda expressions to the TableBuilder.    
        /// </summary>    
        /// <typeparam name="TProperty">Class property that is rendered in the column.</typeparam>    
        /// <param name="expression">Lambda expression identifying a property to be rendered.</param>    
        /// <returns>An instance of TableColumn.</returns>    
        public ITableColumn Expression<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return TableBuilder.AddColumn(expression);
        }
    }

    #endregion ColumnBuilder  
    #region TableBuilder  

    /// <summary>    
    /// Properties and methods used by the consumer to configure the TableBuilder.    
    /// </summary>    
    public interface ITableBuilder<TModel> where TModel : class
    {
        TableBuilder<TModel> DataSource(IEnumerable<TModel> dataSource);
        TableBuilder<TModel> Columns(Action<ColumnBuilder<TModel>> columnBuilder);
    }

    /// <summary>    
    /// Build a table based on an enumerable list of model objects.    
    /// </summary>    
    /// <typeparam name="TModel">Type of model to render in the table.</typeparam>    
    public class TableBuilder<TModel> : ITableBuilder<TModel> where TModel : class
    {
        private HtmlHelper HtmlHelper
        {
            get;
            set;
        }
        private IEnumerable<TModel> Data
        {
            get;
            set;
        }

        /// <summary>    
        /// Default constructor.    
        /// </summary>    
        private TableBuilder() { }

        /// <summary>    
        /// Constructor.    
        /// </summary>    
        internal TableBuilder(HtmlHelper helper)
        {
            this.HtmlHelper = helper;

            this.TableColumns = new List<ITableColumnInternal<TModel>>();
        }

        /// <summary>    
        /// Set the enumerable list of model objects.    
        /// </summary>    
        /// <param name="dataSource">Enumerable list of model objects.</param>    
        /// <returns>Reference to the TableBuilder object.</returns>    
        public TableBuilder<TModel> DataSource(IEnumerable<TModel> dataSource)
        {
            this.Data = dataSource;
            return this;
        }

        /// <summary>    
        /// List of table columns to be rendered in the table.    
        /// </summary>    
        internal IList<ITableColumnInternal<TModel>> TableColumns
        {
            get;
            set;
        }

        /// <summary>    
        /// Add an lambda expression as a TableColumn.    
        /// </summary>    
        /// <typeparam name="TProperty">Model class property to be added as a column.</typeparam>    
        /// <param name="expression">Lambda expression identifying a property to be rendered.</param>    
        /// <returns>An instance of TableColumn.</returns>    
        internal ITableColumn AddColumn<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            TableColumn<TModel, TProperty> column = new TableColumn<TModel, TProperty>(expression);
            this.TableColumns.Add(column);
            return column;
        }

        /// <summary>    
        /// Create an instance of the ColumnBuilder to add columns to the table.    
        /// </summary>    
        /// <param name="columnBuilder">Delegate to create an instance of ColumnBuilder.</param>    
        /// <returns>An instance of TableBuilder.</returns>    
        public TableBuilder<TModel> Columns(Action<ColumnBuilder<TModel>> columnBuilder)
        {
            ColumnBuilder<TModel> builder = new ColumnBuilder<TModel>(this);
            columnBuilder(builder);
            return this;
        }

        /// <summary>    
        /// Convert the TableBuilder to HTML.    
        /// </summary>    
        public MvcHtmlString ToHtml(string id, string CssClass, string EditPath, string DeletePath, bool showHeader = true, bool showStt = true)
        {

            var table = new TagBuilder("table");
            table.GenerateId(id);
            table.AddCssClass(CssClass);
            table.MergeAttribute("role", "grid");



            //For Declaration Of All Require Tag...!!!    
            TagBuilder thead = new TagBuilder("thead");
            TagBuilder tr = new TagBuilder("tr");

            //Hiển thị hay không hiển thị header table
            if (!showHeader)
            {
                tr.MergeAttribute("style", "display:none");
            }
            TagBuilder td = null;
            //TagBuilder th = new TagBuilder("th");    
            TagBuilder th = null;
            TagBuilder tbody = new TagBuilder("tbody");
            //tbody.MergeAttribute("role", "rowgroup");
            //Inner html Of Table...!!!    
            StringBuilder sb = new StringBuilder();
            //Add Headers...!!!    
            //int i = 0;

            var thStt = new TagBuilder("th");
            thStt.InnerHtml = "<span>Stt</span>";
            thStt.AddCssClass("center");
            var thFirst = new TagBuilder("th");
            thFirst.InnerHtml = "<label><input type = \"checkbox\" id = \"chkAll\" class=\"ace\"><span class=\"lbl\"></span></label><input id=\"hidListCheckbox\" name=\"hidListCheckbox\" type=\"hidden\" value=\"\">";
            //thFirst.MergeAttribute("width", "30");
            thFirst.AddCssClass("center");
            if (showStt)
                tr.InnerHtml = thStt.ToString();
            tr.InnerHtml += thFirst.ToString();
            int i = 0;
            var colgroup = new StringBuilder();
            colgroup.AppendLine("<colgroup>");
            if (showStt)
                colgroup.AppendLine("<col style=\"width: 3% \">");
            colgroup.AppendLine("<col style=\"width: 3% \">");
            foreach (ITableColumnInternal<TModel> tc in this.TableColumns)
            {
                th = new TagBuilder("th");

                //th.InnerHtml = tc.ColumnTitle + " <i class='fa fa-ellipsis-v'></i>";
                //if (!string.IsNullOrWhiteSpace(tc.ColumnClass))
                //    th.AddCssClass(tc.ColumnClass);
                //if (!string.IsNullOrWhiteSpace(tc.ColumnWidth))
                //    th.MergeAttribute("width", tc.ColumnWidth);
                //th.MergeAttribute("data-field", tc.ColumnName);
                //th.MergeAttribute("data-sort", "asc"); //mặc định sắp xếp tăng dần
                if (i == 0)
                {
                    th.InnerHtml = tc.ColumnTitle;
                    th.MergeAttribute("style", "display:none;");
                }
                else
                {
                    colgroup.AppendLine(string.Format("<col style=\"width: {0}% \">", !string.IsNullOrWhiteSpace(tc.ColumnWidth) ? tc.ColumnWidth : ""));
                    th.InnerHtml = string.Format("<a href='javascript:void(0);'>{0}  <i class='fa fa-ellipsis-v'></i></a>", tc.ColumnTitle);
                    if (!string.IsNullOrWhiteSpace(tc.ColumnClass))
                        th.AddCssClass(tc.ColumnClass);
                    //if (!string.IsNullOrWhiteSpace(tc.ColumnWidth))
                    //    th.MergeAttribute("width", tc.ColumnWidth);
                    th.MergeAttribute("data-field", tc.ColumnName);
                    th.MergeAttribute("data-sort", "asc"); //mặc định sắp xếp tăng dần
                }
                i++;
                tr.InnerHtml += th.ToString();
            }
            colgroup.AppendLine("</colgroup>");
            //th.InnerHtml = "Action";
            //tr.InnerHtml += th.ToString();
            thead.InnerHtml = tr.ToString();
            sb.Append(colgroup.ToString());
            sb.Append(thead.ToString());

            //For Row Data and Coloumn...!!!    
            if (this.Data != null)
            {
                //var data in RowDetail    
                int row = 0;
                foreach (TModel model in this.Data)
                {
                    if (model != null)
                    {
                        var tdStt = new TagBuilder("td");
                        tdStt.InnerHtml = (row + 1).ToString();
                        tdStt.AddCssClass("center");
                        var tdFirst = new TagBuilder("td");
                        ITableColumnInternal<TModel> tcFirst = this.TableColumns.FirstOrDefault(c => c.ColumnName.ToLower() == "id");

                        tdFirst.InnerHtml = string.Format("<label><input type=\"checkbox\" class=\"ace chkItem\" value=\"{0}\" /><span class=\"lbl\"></span></label>", tcFirst != null ? tcFirst.Evaluate(model) : "");
                        tdFirst.AddCssClass("center");
                        tr = new TagBuilder("tr");
                        if (showStt)
                            tr.InnerHtml += tdStt.ToString();
                        tr.InnerHtml += tdFirst.ToString();
                        var aa = tcFirst != null ? tcFirst.Evaluate(model) : "";
                        tr.MergeAttribute("idata", aa);
                        tr.MergeAttribute("scope", "row");
                        tr.AddCssClass("trItem"); //dùng để chọn click view detailbox
                        //var header in Headernames    
                        int j = 0;

                        foreach (ITableColumnInternal<TModel> tc in this.TableColumns)
                        {

                            td = new TagBuilder("td");

                            //td.InnerHtml = tc.Evaluate(model);
                            //td.AddCssClass(tc.ColumnClass);

                            if (j == 0)
                            {
                                //ID = tc.Evaluate(model);
                                td.InnerHtml = tc.Evaluate(model);
                                td.MergeAttribute("style", "display:none;");
                            }
                            else
                            {
                                if (tc.ColumnName.Equals("Active"))
                                {
                                    if (tc.Evaluate(model).Equals("True"))
                                    {
                                        td.InnerHtml = "<span class=\"label label-sm label-success\">Kích hoạt</span>";
                                    }
                                    else
                                    {
                                        td.InnerHtml = "<span class=\"label label-sm label-warning\">Chưa kích hoạt</span>";
                                    }
                                }
                                else if (tc.ColumnName.Equals("DateCreated"))
                                {
                                    td.InnerHtml = Convert.ToDateTime(tc.Evaluate(model)).ToString("dd/MM/yyyy HH:mm");
                                }
                                else
                                {
                                    td.InnerHtml = tc.Evaluate(model);
                                }
                                td.AddCssClass(tc.ColumnClass);
                            }
                            tr.InnerHtml += td.ToString();

                            j++;
                        }
                        //td.InnerHtml = "<a href='" + EditPath + ID + "'>edit</a> <a href='" + DeletePath + ID + "'>delete</a>";
                        //tr.InnerHtml += td.ToString();
                        tbody.InnerHtml += tr.ToString();
                        row++;
                    }
                }

                sb.Append(tbody.ToString());
                table.InnerHtml = sb.ToString();
            }
            return new MvcHtmlString(table.ToString());

        }
        public MvcHtmlString ToHtmlUpdate(string id, string CssClass, string EditPath, string DeletePath, bool showHeader = true, bool showStt = true)
        {

            var table = new TagBuilder("table");
            table.GenerateId(id);
            table.AddCssClass(CssClass);

            //For Declaration Of All Require Tag...!!!    
            TagBuilder thead = new TagBuilder("thead");
            TagBuilder tr = new TagBuilder("tr");
            //Hiển thị hay không hiển thị header table
            if (!showHeader)
            {
                tr.MergeAttribute("style", "display:none");
            }
            TagBuilder td = null;
            //TagBuilder th = new TagBuilder("th");    
            TagBuilder th = null;
            TagBuilder tbody = new TagBuilder("tbody");
            //tbody.MergeAttribute("role", "rowgroup");
            //Inner html Of Table...!!!    
            StringBuilder sb = new StringBuilder();
            //Add Headers...!!!    
            //int i = 0;
            var thStt = new TagBuilder("th");
            thStt.InnerHtml = "<span>Stt</span>";
            thStt.AddCssClass("center");
            var thFirst = new TagBuilder("th");
            thFirst.InnerHtml = "<label><input type = \"checkbox\" id = \"chkAll\" class=\"ace\"><span class=\"lbl\"></span></label><input id=\"hidListCheckbox\" name=\"hidListCheckbox\" type=\"hidden\" value=\"\">";
            //thFirst.MergeAttribute("width", "30");
            thFirst.AddCssClass("center");
            if (showStt)
                tr.InnerHtml = thStt.ToString();
            tr.InnerHtml += thFirst.ToString();
            int i = 0;
            var colgroup = new StringBuilder();
            colgroup.AppendLine("<colgroup>");
            if (showStt)
                colgroup.AppendLine("<col style=\"width: 3% \">");
            colgroup.AppendLine("<col style=\"width: 3% \">");
            foreach (ITableColumnInternal<TModel> tc in this.TableColumns)
            {
                th = new TagBuilder("th");

                //C#1-------------------
                //th.InnerHtml = tc.ColumnTitle + " <i class='fa fa-ellipsis-v'></i>";
                //if (!string.IsNullOrWhiteSpace(tc.ColumnClass))
                //    th.AddCssClass(tc.ColumnClass);
                //if (!string.IsNullOrWhiteSpace(tc.ColumnWidth))
                //    th.MergeAttribute("width", tc.ColumnWidth);
                //th.MergeAttribute("data-field", tc.ColumnName);
                //th.MergeAttribute("data-sort", "asc"); //mặc định sắp xếp tăng dần

                //C#2------------------------
                //if (i == 0)
                //{
                //    th.InnerHtml = tc.ColumnTitle;
                //    //th.MergeAttribute("style", "display:none;");
                //}
                //else
                //{
                //    colgroup.AppendLine(string.Format("<col style=\"width: {0}% \">", !string.IsNullOrWhiteSpace(tc.ColumnWidth) ? tc.ColumnWidth : ""));
                //    th.InnerHtml = string.Format("<a href='javascript:void(0);'><label>{0}</label>  <i class='fa fa-ellipsis-v'></i></a>", tc.ColumnTitle);
                //    if (!string.IsNullOrWhiteSpace(tc.ColumnClass))
                //        th.AddCssClass(tc.ColumnClass);
                //    //if (!string.IsNullOrWhiteSpace(tc.ColumnWidth))
                //    //    th.MergeAttribute("width", tc.ColumnWidth);
                //    th.MergeAttribute("data-field", tc.ColumnName);
                //    th.MergeAttribute("data-sort", "asc"); //mặc định sắp xếp tăng dần
                //}

                colgroup.AppendLine(string.Format("<col style=\"width: {0}% \">", !string.IsNullOrWhiteSpace(tc.ColumnWidth) ? tc.ColumnWidth : ""));
                th.InnerHtml = string.Format("<a href='javascript:void(0);'><label>{0}</label>  <i class='fa fa-ellipsis-v'></i></a>", tc.ColumnTitle);
                if (!string.IsNullOrWhiteSpace(tc.ColumnClass))
                    th.AddCssClass(tc.ColumnClass);
                //if (!string.IsNullOrWhiteSpace(tc.ColumnWidth))
                //    th.MergeAttribute("width", tc.ColumnWidth);
                th.MergeAttribute("data-field", tc.ColumnName);
                th.MergeAttribute("data-sort", "asc"); //mặc định sắp xếp tăng dần

                //i++;
                if (showHeader)
                    tr.InnerHtml += th.ToString();
            }
            colgroup.AppendLine("<col style=\"width: 10% \">");
            colgroup.AppendLine("</colgroup>");
            th.InnerHtml = "<label>Thao tác</label>";
            tr.InnerHtml += th.ToString();
            thead.InnerHtml = tr.ToString();
            sb.Append(colgroup.ToString());
            sb.Append(thead.ToString());

            //For Row Data and Coloumn...!!!    
            if (this.Data != null)
            {
                //var data in RowDetail    
                int row = 0;
                foreach (TModel model in this.Data)
                {
                    if (model != null)
                    {
                        var tdStt = new TagBuilder("td");
                        tdStt.InnerHtml = (row + 1).ToString();
                        tdStt.AddCssClass("center");
                        var tdFirst = new TagBuilder("td");
                        ITableColumnInternal<TModel> tcFirst = this.TableColumns.FirstOrDefault(c => c.ColumnName.ToLower() == "id");

                        tdFirst.InnerHtml = string.Format("<label><input type=\"checkbox\" class=\"ace chkItem\" value=\"{0}\" /><span class=\"lbl\"></span></label>", tcFirst != null ? tcFirst.Evaluate(model) : "");
                        tdFirst.AddCssClass("center");
                        tr = new TagBuilder("tr");
                        if (showStt)
                            tr.InnerHtml += tdStt.ToString();
                        tr.InnerHtml += tdFirst.ToString();
                        var aa = tcFirst != null ? tcFirst.Evaluate(model) : "";
                        tr.MergeAttribute("idata", aa);
                        tr.MergeAttribute("scope", "row");
                        tr.AddCssClass("trItem"); //dùng để chọn click view detailbox
                        //var header in Headernames    
                        int j = 0;
                        string ID = "";
                        foreach (ITableColumnInternal<TModel> tc in this.TableColumns)
                        {
                            td = new TagBuilder("td");

                            //td.InnerHtml = tc.Evaluate(model);
                            //td.AddCssClass(tc.ColumnClass);
                            
                            if (j == 0)
                            {
                                ID = tc.Evaluate(model);

                                td.InnerHtml = tc.Evaluate(model);
                                
                                //td.MergeAttribute("style", "display:none;");
                            }
                            else
                            {
                                var chkNum = Regex.IsMatch(tc.Evaluate(model), @"^\d+$");
                                var _contentColumn = tc.Evaluate(model);
                                if(chkNum)
                                    _contentColumn = Convert.ToDecimal(tc.Evaluate(model)).ToString("##,###");
                                if (tc.ColumnName.Equals("Active"))
                                {
                                    if (tc.Evaluate(model).Equals("True"))
                                    {
                                        td.InnerHtml = "<span class=\"label label-sm label-success\">Kích hoạt</span>";
                                    }
                                    else
                                    {
                                        td.InnerHtml = "<span class=\"label label-sm label-warning\">Chưa kích hoạt</span>";
                                    }
                                }
                                else if (tc.ColumnName.Equals("DateCreated"))
                                {
                                    td.InnerHtml = Convert.ToDateTime(tc.Evaluate(model)).ToString("dd/MM/yyyy HH:mm");
                                }
                                else
                                {
                                    td.InnerHtml = _contentColumn;
                                }
                                td.AddCssClass(tc.ColumnClass);
                            }
                            tr.InnerHtml += td.ToString();

                            j++;
                        }
                        td.InnerHtml = "<a href='" + EditPath + ID + "' idata='" + ID + "' name='btnUpdate'><i class=\"ace-icon fa fa-pencil bigger-120\"></i></a> <a href='javascript:void(0);' idata='" + ID + "' name='btnDelete' class='red btnDelete'><i class=\"ace-icon fa fa-trash bigger-120\"></i></a>";
                        tr.InnerHtml += td.ToString();
                        tbody.InnerHtml += tr.ToString();
                        row++;
                    }
                }

                sb.Append(tbody.ToString());
                table.InnerHtml = sb.ToString();
            }
            return new MvcHtmlString(table.ToString());

        }
    }

    #endregion TableBuilder

    #region MvcHtmlTableExtensions  

    public static class MvcHtmlTableExtensions
    {
        /// <summary>    
        /// Return an instance of a TableBuilder.    
        /// </summary>    
        /// <typeparam name="TModel">Type of model to render in the table.</typeparam>    
        /// <returns>Instance of a TableBuilder.</returns>    
        public static ITableBuilder<TModel> TableFor<TModel>(this HtmlHelper helper) where TModel : class
        {
            return new TableBuilder<TModel>(helper);
        }
    }

    #endregion MvcHtmlTableExtensions 
}
