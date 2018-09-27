using Hk.Core.Business.OrganSet;
using Hk.Core.Entity.OrganSet;
using Hk.Core.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;

namespace Hk.Core.Web
{
    [Area("OrganSet")]
    public class PBDM_ORGANController : BaseMvcController
    {
        PBDM_ORGANBusiness _pBDM_ORGANBusiness = new PBDM_ORGANBusiness();

        #region 视图功能

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new PBDM_ORGAN() : _pBDM_ORGANBusiness.GetTheData(id);

            return View(theData);
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
            var dataList = _pBDM_ORGANBusiness.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public IActionResult SaveData(PBDM_ORGAN theData)
        {
            if(theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                _pBDM_ORGANBusiness.AddData(theData);
            }
            else
            {
                _pBDM_ORGANBusiness.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public IActionResult DeleteData(string ids)
        {
            _pBDM_ORGANBusiness.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}