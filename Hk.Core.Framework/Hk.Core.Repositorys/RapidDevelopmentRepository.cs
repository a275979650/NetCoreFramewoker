using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Hk.Core.Util.Helper;
using Hk.Core.Util.Model;
using Microsoft.AspNetCore.Hosting;

namespace Hk.Core.Repositorys
{
    public class RapidDevelopmentRepository:BaseRepository<Base_DatabaseLink,string>, IRapidDevelopmentRepository
    {
        public RapidDevelopmentRepository(IDbContextCore dbContext, IHostingEnvironment hostingEnvironment) : base(dbContext)
        {
            _contentRootPath = hostingEnvironment.ContentRootPath;
        }

        public List<Base_DatabaseLink> GetAllDbLink()
        {
            return Get().ToList();
        }

        public List<DbTableInfo> GetDbTableList(string linkId)
        {
            if (linkId.IsNullOrEmpty())
                return new List<DbTableInfo>();
            else
                return GetTheDbHelper(linkId).GetDbAllTables();
        }
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="linkId">连接Id</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tables">表列表</param>
        /// <param name="buildType">需要生成类型</param>
        public void BuildCode(string linkId, string areaName, string tables, string buildType)
        {
            //内部成员初始化
            _dbHelper = GetTheDbHelper(linkId);
            GetDbTableList(linkId).ForEach(aTable => { _dbTableInfoDic.Add(aTable.TableName, aTable); });

            List<string> tableList = tables.ToList<string>();
            List<string> buildTypeList = buildType.ToList<string>();
            tableList.ForEach(aTable =>
            {
                var tableFieldInfo = _dbHelper.GetDbTableInfo(aTable);
                //实体层
                if (buildTypeList.Exists(x => x.ToLower() == "entity"))
                {
                    BuildEntity(tableFieldInfo, areaName, aTable);
                }

                //业务层
                if (buildTypeList.Exists(x => x.ToLower() == "business"))
                {;
                    BuildIRepository(areaName,aTable);
                    BuildRepository(areaName,aTable);
                }

                //控制器
                if (buildTypeList.Exists(x => x.ToLower() == "controller"))
                {
                    BuildController(areaName, aTable);
                }

                //视图
                if (buildTypeList.Exists(x => x.ToLower() == "view"))
                {
                    BuildView(tableFieldInfo, areaName, aTable);
                }
            });
        }
        #region 私有成员

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="tableInfo">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tableName">表名</param>
        private void BuildEntity(List<TableInfo> tableInfo, string areaName, string tableName)
        {
            string entityPath = _contentRootPath.Replace("Hk.Core.Web", "Hk.Core.Entity");
            string filePath = Path.Combine(entityPath, areaName, $"{tableName.ToCamelFirstNameUpper()}.cs");
            string nameSpace = $@"Hk.Core.Entity.{areaName}";

            _dbHelper.SaveEntityToFile(tableInfo, tableName, _dbTableInfoDic[tableName].Description, filePath,
                nameSpace);
        }

        /// <summary>
        /// 生成存储接口
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="entityName"></param>
        private void BuildIRepository(string areaName, string entityName)
        {
            entityName = entityName.ToCamelFirstNameUpper();
            #region 代码构造
            string code =
                $@"using System.Collections.Generic;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.{areaName};
using Hk.Core.Util.Datas;
using Hk.Core.Util.Dependency;
namespace Hk.Core.IRepositorys.{areaName}
{{
    public interface I{entityName}Repository:IRepository<{entityName},string>,IScopeDependency
    {{
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name=""condition"">查询类型</param>
        /// <param name=""keyword"">关键字</param>
        /// <returns></returns>
        List<{entityName}> GetDataList(string condition, string keyword, Pagination pagination);
        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name=""id"">主键</param>
        /// <returns></returns>
        {entityName} GetTheData(string id);
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name=""newData"">数据</param>
        void AddData({entityName} newData);
        /// <summary>
        /// 更新数据
        /// </summary>
        void UpdateData({entityName} theData);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""theData"">删除的数据</param>
        void DeleteData(List<string> ids);
    }}
}}";


            #endregion
            string businessPath = _contentRootPath.Replace("Hk.Core.Web", "Hk.Core.IRepositorys");
            string filePath = Path.Combine(businessPath, areaName, $"I{entityName}Repository.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }
        /// <summary>
        /// 生成存储实现
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="entityName"></param>
        private void BuildRepository(string areaName, string entityName)
        {
            entityName = entityName.ToCamelFirstNameUpper();
            #region 代码构造
            string code =
$@"using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Data.Repositories;
using Hk.Core.Entity.{areaName};
using Hk.Core.IRepositorys.{areaName};
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
namespace Hk.Core.Repositorys.{areaName}
{{
    public class {entityName}Repository:BaseRepository<{entityName},string>,I{entityName}Repository
    {{
        public {entityName}Repository(IDbContextCore dbContext) : base(dbContext)
        {{
        }}
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name=""condition"">查询类型</param>
        /// <param name=""keyword"">关键字</param>
        /// <returns></returns>
        public List<{entityName}> GetDataList(string condition, string keyword, Pagination pagination)
        {{
            var q = Get();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@""{{condition}}.Contains(@0)"", keyword);

            return q.GetPagination(pagination).ToList();
        }}

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name=""id"">主键</param>
        /// <returns></returns>
        public {entityName} GetTheData(string id)
        {{
             return GetSingle(id);
        }}

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name=""newData"">数据</param>
        public void AddData({entityName} newData)
        {{
            Add(newData);
        }}

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData({entityName} theData)
        {{
            Update(theData);
        }}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""theData"">删除的数据</param>
        public void DeleteData(List<string> ids)
        {{
            ids.ForEach(x => Delete(x));
        }}

        #endregion

        #region 私有成员

        #endregion
    }}
}}";


            #endregion

            string businessPath = _contentRootPath.Replace("Hk.Core.Web", "Hk.Core.Repositorys");
            string filePath = Path.Combine(businessPath, areaName, $"{entityName}Repository.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }
        /// <summary>
        /// 生成控制器代码
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildController(string areaName, string entityName)
        {
            string varName = $@"_{entityName.ToCamelFirstNameLower()}Repository";
            string interfaceName = $"I{entityName.ToCamelFirstNameUpper()}Repository";
            string valName = $"{entityName.ToCamelFirstNameLower()}Repository";
            entityName = entityName.ToCamelFirstNameUpper();
            #region 代码构造
            string code =
$@"using System;
using Hk.Core.IRepositorys.{areaName};
using Hk.Core.Entity.{areaName};
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Microsoft.AspNetCore.Mvc;
namespace Hk.Core.Web
{{
    [Area(""{areaName}"")]
    public class {entityName}Controller : BaseMvcController
    {{
        private readonly {interfaceName} {varName};
        public {entityName}Controller({interfaceName} {valName})
        {{
            {varName} = {valName};
        }}
        #region 视图功能

        public IActionResult Index()
        {{
            return View();
        }}

        public IActionResult Form(string id)
        {{
            var theData = id.IsNullOrEmpty() ? new {entityName}() : {varName}.GetTheData(id);

            return View(theData);
        }}

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name=""condition"">查询类型</param>
        /// <param name=""keyword"">关键字</param>
        /// <returns></returns>
        public IActionResult GetDataList(string condition, string keyword, Pagination pagination)
        {{
            var dataList = {varName}.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }}

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name=""theData"">保存的数据</param>
        public IActionResult SaveData({entityName} theData)
        {{
            if(theData.Id.IsNullOrEmpty())
            {{
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                {varName}.AddData(theData);
            }}
            else
            {{
                {varName}.UpdateData(theData);
            }}

            return Success();
        }}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name=""theData"">删除的数据</param>
        public IActionResult DeleteData(string ids)
        {{
            {varName}.DeleteData(ids.ToList<string>());

            return Success(""删除成功！"");
        }}

        #endregion
    }}
}}";


            #endregion

            string filePath = Path.Combine(_contentRootPath, "Areas", areaName, "Controllers",
                $"{entityName}Controller.cs");

            FileHelper.WriteTxt(code, filePath, FileMode.Create);
        }

        /// <summary>
        /// 生成视图
        /// </summary>
        /// <param name="tableInfoList">表字段信息</param>
        /// <param name="areaName">区域名</param>
        /// <param name="entityName">实体名</param>
        private void BuildView(List<TableInfo> tableInfoList, string areaName, string entityName)
        {
            entityName = entityName.ToCamelFirstNameUpper();
            //生成Index页面
            StringBuilder searchConditionSelectHtml = new StringBuilder();
            StringBuilder tableColsBuilder = new StringBuilder();
            StringBuilder formRowBuilder = new StringBuilder();
            int count = 0;
            tableInfoList.Where(x => x.Name != "Id").ForEach((aField, index) =>
            {
                count++;
                //搜索的下拉选项
                Type fieldType = _dbHelper.DbTypeStr_To_CsharpType(aField.Type);
                if (fieldType == typeof(string))
                {
                    string newOption = $@"
                <option value=""{aField.Name}"">{aField.Description}</option>";
                    searchConditionSelectHtml.Append(newOption);
                }

                //数据表格列
                string end = (index == tableInfoList.Count - 2) ? "" : ",";
                string newCol = $@"
                {{ title: '{aField.Description}', field: '{aField.Name.ToCamelFirstNameUpper()}', width: 200 }}{end}";
                tableColsBuilder.Append(newCol);

                //Form页面中的Html
                //    string newFormRow = $@"
                //<tr>
                //    <th>{aField.Description}</th>
                //    <td>
                //        <input name=""{aField.Name}"" value=""@obj.{aField.Name}"" class=""easyui-textbox"" data-options=""width:'200px',required:true"">
                //    </td>
                //</tr>";
                StringBuilder formStringBuilder = new StringBuilder();
                //formStringBuilder.AppendLine("<tr>");
                formStringBuilder.AppendLine($@"<th>{aField.Description}</th>");
                formStringBuilder.AppendLine("<td>");
                formStringBuilder.AppendLine(
                    $@"   <input name=""{aField.Name}"" value=""@obj.{
                            aField.Name.ToCamelFirstNameUpper()
                        }"" class=""easyui-textbox"" data-options=""width:'200px',required:true"">");
                formStringBuilder.AppendLine("</td>");
                if (tableInfoList.Count > 10)
                {
                    if (count % 2 == 1)
                    {
                        formStringBuilder.Insert(0, "<tr>\r\n");
                    }
                    else if (count % 2 == 0)
                    {
                        formStringBuilder.AppendLine("</tr>");
                    }
                }
                else
                {
                    formStringBuilder.Insert(0, "<tr>\r\n");
                    formStringBuilder.AppendLine("</tr>");
                }

                //formStringBuilder.AppendLine("</tr>");
                formRowBuilder.Append(formStringBuilder.ToString());
            });
            int maxHeight = (tableInfoList.Count - 1) * 35 + 200;
            maxHeight = maxHeight > 800 ? 800 : maxHeight;
            int maxWidth = tableInfoList.Count > 10 ? 800 : 500;

            #region index代码构造
            string indexHtml =
$@"@{{
    Layout = ""~/Views/Shared/_Layout_List.cshtml"";
}}

@section toolbar{{
    <a id=""add"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-add'"">添加</a>
    <a id=""edit"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-edit'"">修改</a>
    <a id=""delete"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-remove'"">删除</a>
}}

@section search{{
    <div class=""search_wrapper"">
        <div class=""search_item"">
            <label class=""search_label"">查询类别</label>
            <select name=""condition"" class=""easyui-combobox"" data-options=""width:100"">
                <option value="""">请选择</option>
                {searchConditionSelectHtml.ToString()}
            </select>
            <input name=""keyword"" class=""easyui-textbox"" style=""width:150px"" />
        </div>
        <div class=""search_submit"">
            <a href=""javascript:;"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-search'"" onclick=""searchGrid(this,'#dataTable')"">查询</a>
        </div>
    </div>
}}
<div id=""dataTable"">

</div>

<script>
    var rootUrl = '@Url.Content(""~/"")';
    var formWidth = {maxWidth};
    var formHeight = {maxHeight};

    function initTable() {{
        $('#dataTable').datagrid({{
            url: rootUrl + '{areaName}/{entityName}/GetDataList',
            method: 'POST',
            //queryParams: {{ 'id': id }},
            idField: 'Id',
            fit: true,
            fitColumns: true,
            singleSelect: false,
            selectOnCheck: false,
            checkOnSelect: false,
            //rownumbers: true,
            pagination: true,
            pageSize: 30,
            //nowrap: false,
            pageList: [10, 20, 30, 50, 100, 150, 200],
            //showFooter: true,
            columns: [[
                {{ title: 'ck', field: 'ck', checkbox: true }},
                {tableColsBuilder.ToString()}
            ]],
            onBeforeLoad: function (param) {{

            }},
            onBeforeSelect: function () {{
                return false;
            }}
        }});
    }}

    $(function () {{
        initTable();

        //添加数据
        $('#add').click(function () {{
            dialogOpen({{
                id: 'form',
                title: '添加数据',
                width: formWidth,
                height: formHeight,
                url: rootUrl + '{areaName}/{entityName}/Form',
            }});
        }});

        //修改数据
        $('#edit').click(function () {{
            var selected = $(""#dataTable"").datagrid(""getChecked"");
            if (!selected || !selected.length) {{
                dialogError('请选择要修改的记录!');
                return;
            }}
            var id = selected[0].Id;

            dialogOpen({{
                id: 'form',
                title: '修改数据',
                width: formWidth,
                height: formHeight,
                url: rootUrl + '{areaName}/{entityName}/Form?id=' + id,
            }});
        }});

        //删除数据
        $('#delete').click(function () {{
            var checked = $(""#dataTable"").datagrid(""getChecked"");
            if (!checked || !checked.length) {{
                dialogError('请选择要删除的记录!');
                return;
            }}
            var ids = $.map(checked, function (item) {{
                return item['Id'];
            }});

            dialogComfirm('确认删除吗？', function () {{
                $.postJSON(rootUrl + '{areaName}/{
                       entityName
                   }/DeleteData', {{ ids: JSON.stringify(ids) }}, function (resJson) {{
                    if (resJson.Success) {{
                        $('#dataTable').datagrid('clearSelections').datagrid('clearChecked');
                        $('#dataTable').datagrid('reload');
                        dialogMsg('删除成功!');
                    }}
                    else {{
                        dialogError(resJson.Msg);
                    }}
                }});
            }});
        }});
    }});
</script>";


            #endregion

            string indexPath = Path.Combine(_contentRootPath, "Areas", areaName, "Views", entityName, "Index.cshtml");

            FileHelper.WriteTxt(indexHtml, indexPath, FileMode.Create);

            #region Form页面代码构造
            string formHtml =
                $@" @using Hk.Core.Entity.{areaName};
@using Hk.Core.Util;
@using Hk.Core.Util.Extentions
@{{
    Layout = ""~/Views/Shared/_Layout_List.cshtml"";

    var obj = ({entityName})Model;
    var objStr = Html.Raw(obj.ToJson());
}}

<form id=""dataForm"" enctype=""multipart/form-data"" class=""easyui-form"" method=""post"" data-options=""novalidate:true"">
    <table class=""table_base"">
        <colgroup>
            <col style=""width:80px;"" />
        </colgroup>
        <tbody>
            {formRowBuilder.ToString()}
        </tbody>
    </table>
</form>

@section foottoolbar{{
    <a id=""saveForm"" href=""javascript:;"" class=""easyui-linkbutton"" data-options=""iconCls:'icon-save'"">保存</a>
}}

<script>
    var rootUrl = '@Url.Content(""~/"")';
    var theEntity = @objStr;

    $(function () {{
        $('#saveForm').click(function () {{
            if (!$('#dataForm').form('enableValidation').form('validate'))
                return;

            var formValues = $('#dataForm').getValues();
            $.extend(theEntity, formValues);
            $.postJSON(rootUrl + '{areaName}/{entityName}/SaveData', theEntity, function (resJson) {{
                if (resJson.Success) {{
                    parent.dialogMsg('保存成功!');
                    parent.$('#dataTable').datagrid('reload');
                    parent.dialogClose('form');
                }}
                else {{
                    dialogError(resJson.Msg);
                }}
            }});
        }});
    }});
</script>
";


            #endregion
            //生成Form页面

            string formPath = Path.Combine(_contentRootPath, "Areas", areaName, "Views", entityName, "Form.cshtml");

            FileHelper.WriteTxt(formHtml, formPath, FileMode.Create);
        }

        /// <summary>
        /// 获取对应的数据库帮助类
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        private DbHelper GetTheDbHelper(string linkId)
        {
            var theLink = GetTheLink(linkId);
            DbHelper dbHelper = DbHelperFactory.GetDbHelper(theLink.DbType, theLink.ConnectionStr);

            return dbHelper;
        }

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="linkId">连接Id</param>
        /// <returns></returns>
        private Base_DatabaseLink GetTheLink(string linkId)
        {
            Base_DatabaseLink resObj = new Base_DatabaseLink();
            var theModule = Get().FirstOrDefault(x => x.Id == linkId);
            resObj = theModule ?? resObj;

            return resObj;
        }

        private DbHelper _dbHelper { get; set; }

        private Dictionary<string, DbTableInfo> _dbTableInfoDic { get; set; } = new Dictionary<string, DbTableInfo>();

        private string _contentRootPath;

        #endregion
    }
}