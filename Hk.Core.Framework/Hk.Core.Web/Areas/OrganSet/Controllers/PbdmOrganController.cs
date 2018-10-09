using System;
using Hk.Core.IRepositorys.OrganSet;
using Hk.Core.Entity.OrganSet;
using Hk.Core.Util.Datas;
using Hk.Core.Util.Extentions;
using Microsoft.AspNetCore.Mvc;
namespace Hk.Core.Web
{
    [Area("OrganSet")]
    public class PbdmOrganController : BaseMvcController
    {
        private readonly IPbdmOrganRepository _pbdmOrganRepository;
        public PbdmOrganController(IPbdmOrganRepository pbdmOrganRepository)
        {
            _pbdmOrganRepository = pbdmOrganRepository;
        }
        #region 视图功能

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new PbdmOrgan() : _pbdmOrganRepository.GetTheData(id);

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
            var dataList = _pbdmOrganRepository.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public IActionResult SaveData(PbdmOrgan theData)
        {
            if(theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                _pbdmOrganRepository.AddData(theData);
            }
            else
            {
                _pbdmOrganRepository.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public IActionResult DeleteData(string ids)
        {
            _pbdmOrganRepository.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}