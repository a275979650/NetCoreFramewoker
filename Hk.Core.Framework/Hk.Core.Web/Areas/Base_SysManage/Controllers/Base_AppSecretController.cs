using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Hk.Core.Web.Common;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Hk.Core.Web
{
    [Area("Base_SysManage")]
    public class Base_AppSecretController : BaseMvcController
    {
        private readonly IBaseAppSecretRepository _baseAppSecretRepository;
        public Base_AppSecretController(IBaseAppSecretRepository baseAppSecretRepository)
        {
            _baseAppSecretRepository = baseAppSecretRepository;
        }


        #region 视图功能

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_AppSecret() : _baseAppSecretRepository.GetTheData(id);

            return View(theData);
        }

        public IActionResult PermissionForm(string appId)
        {
            ViewData["appId"] = appId;

            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public IActionResult GetDataList(string condition, string keyword, Pagination pagination)
        {
            var dataList = _baseAppSecretRepository.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public IActionResult SaveData(Base_AppSecret theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                _baseAppSecretRepository.AddData(theData);
            }
            else
            {
                _baseAppSecretRepository.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public IActionResult DeleteData(string ids)
        {
            _baseAppSecretRepository.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        public IActionResult SavePermission(string appId, string permissions)
        {
            PermissionManage.SetAppIdPermission(appId, permissions.ToList<string>());

            return Success();
        }

        #endregion
    }
}