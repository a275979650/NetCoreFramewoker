using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Hk.Core.Web.Common;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hk.Core.Web.Areas.Base_SysManage.Controllers
{
    [Area("Base_SysManage")]
    public class Base_SysRoleController : BaseMvcController
    {
        private readonly IBaseSysRoleRepository _baseSysRoleRepository;
        private readonly IBasePermissionRoleRepository _basePermissionRoleRepository;
        public Base_SysRoleController(IBaseSysRoleRepository baseSysRoleRepository,
            IBasePermissionRoleRepository basePermissionRoleRepository)
        {
            _baseSysRoleRepository = baseSysRoleRepository;
            _basePermissionRoleRepository = basePermissionRoleRepository;
        }

        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_SysRole() : _baseSysRoleRepository.GetTheData(id);

            return View(theData);
        }

        public ActionResult PermissionForm(string roleId)
        {
            ViewData["roleId"] = roleId;

            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public ActionResult GetDataList(string condition, string keyword, Pagination pagination)
        {
            var dataList = _baseSysRoleRepository.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        /// <summary>
        /// 获取角色列表
        /// 注：无分页
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataList_NoPagin()
        {
            Pagination pagination = new Pagination
            {
                PageIndex = 1,
                PageRows = int.MaxValue
            };
            var dataList = _baseSysRoleRepository.GetDataList(null, null, pagination);

            return Content(dataList.ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_SysRole theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();
                theData.RoleId = Guid.NewGuid().ToSequentialGuid();

                _baseSysRoleRepository.AddData(theData);
            }
            else
            {
                _baseSysRoleRepository.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _baseSysRoleRepository.DeleteData(ids.ToList<string>());

            PermissionManage.ClearUserPermissionCache();

            return Success("删除成功！");
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissions">权限值</param>
        /// <returns></returns>
        public ActionResult SavePermission(string roleId, string permissions)
        {
            _basePermissionRoleRepository.SavePermission(roleId, permissions.ToList<string>());

            PermissionManage.ClearUserPermissionCache();

            return Success();
        }
        #endregion
    }
}