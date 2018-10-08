using System;
using Hk.Core.Business.Base_SysManage;
using Hk.Core.Data.DbContextCore;
using Hk.Core.Entity.Base_SysManage;
using Hk.Core.IRepositorys;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace Hk.Core.Web.Areas.Base_SysManage.Controllers
{
    [Area("Base_SysManage")]
    public class Base_DatabaseLinkController : BaseMvcController
    {
        private readonly IBaseDatabaseLinkRepository _baseDatabaseLinkRepository;

        public Base_DatabaseLinkController(IDbContextCore dbContext,IBaseDatabaseLinkRepository baseDatabaseLinkRepository)
        {
            _baseDatabaseLinkRepository = baseDatabaseLinkRepository;
        }
        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_DatabaseLink() : _baseDatabaseLinkRepository.GetTheData(id);

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
        public ActionResult GetDataList(string condition, string keyword, Pagination pagination)
        {
            var dataList = _baseDatabaseLinkRepository.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_DatabaseLink theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                _baseDatabaseLinkRepository.AddData(theData);
            }
            else
            {
                _baseDatabaseLinkRepository.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _baseDatabaseLinkRepository.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}